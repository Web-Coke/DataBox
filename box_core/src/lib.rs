use std::{
    ffi::c_void,
    marker::PhantomData,
    mem::transmute,
    ptr::{slice_from_raw_parts, slice_from_raw_parts_mut},
    str,
    sync::LazyLock,
};

mod _test;
mod geo;
mod ribbon;
mod text;
mod time;

static VERSION: LazyLock<String> = LazyLock::new(|| {
    let mut version = env!("CARGO_PKG_VERSION").as_bytes().to_vec();
    version.push(0);
    if let [.., dot, end] = &mut version[..] {
        *end = *dot;
        *dot = 46;
    };
    unsafe { String::from_utf8_unchecked(version) }
});

#[macro_export]
macro_rules! func {
    () => {{
        fn _type_name<T:Fn()>(_: T) -> &'static str {
            std::any::type_name::<T>()
        }
        let _type_name = _type_name(||{});
        &_type_name[.._type_name.len() - 13]
    }};
}

pub struct Iterators<'a> {
    ptr: *const u8,
    len: *const u8,
    ___: PhantomData<&'a u8>,
}

impl<'a> Iterators<'a> {
    pub fn new(slice: &'a [u8]) -> Self {
        let end = slice.is_ascii().then_some(slice.len()).unwrap();
        let ptr = slice.as_ptr();
        unsafe {
            let len = ptr.add(end);
            Self {
                ptr,
                len,
                ___: PhantomData,
            }
        }
    }

    pub fn next_number(&mut self) -> Option<u64> {
        unsafe {
            let start_ptr = self.ptr;
            let mut len = 0;
            while self.ptr < self.len {
                let mut num = *self.ptr;
                self.ptr = self.ptr.add(1);
                if (0x30..=0x39).contains(&num) {
                    len += 1;
                } else {
                    while !(0x30..=0x39).contains(&num) {
                        num = *self.ptr;
                        self.ptr = self.ptr.add(1);
                    }
                    self.ptr = self.ptr.sub(1);
                    if len == 0 {
                        continue;
                    }
                    return Some(
                        transmute::<&[u8], &str>(&*slice_from_raw_parts(start_ptr, len))
                            .parse::<u64>()
                            .unwrap(),
                    );
                }
            }
            if len == 0 {
                None
            } else {
                Some(
                    transmute::<&[u8], &str>(&*slice_from_raw_parts(start_ptr, len))
                        .parse::<u64>()
                        .unwrap(),
                )
            }
        }
    }
}

#[repr(C)]
pub struct RefObj {
    len: i32,
    // |rty| size |drop
    // | 0 |RefObj|Rust
    // | 1 |8byet |Rust
    // | 2 |16byte|Rust
    // | 4 |32byte|Rust
    // | 8 |64byte|Rust
    // |-1 |   _  |C#
    rty: i32,
    ptr: *mut c_void,
}

impl RefObj {
    // RefArr{len, ptr}
    #[inline(always)]
    pub fn from_str<T: AsRef<str>>(val: T) -> Self {
        let utf_16 = val.as_ref().encode_utf16().collect::<Vec<u16>>();
        Self {
            len: i32::try_from(utf_16.len()).unwrap(),
            rty: 2,
            ptr: Box::into_raw(utf_16.into_boxed_slice()) as *mut c_void,
        }
    }

    #[inline(always)]
    pub fn into_str(self) -> String {
        unsafe {
            String::from_utf16(&*slice_from_raw_parts(
                self.ptr as *mut u16,
                self.len as usize,
            ))
            .unwrap()
        }
    }

    // RefArr{len, RefArr{len, ptr}}
    #[inline(always)]
    pub fn from_str_arr<T: AsRef<str>>(val: Vec<T>) -> Self {
        let val: Vec<Self> = val
            .into_iter()
            .map(|i| {
                let utf_16 = i.as_ref().encode_utf16().collect::<Vec<u16>>();
                Self {
                    len: i32::try_from(utf_16.len()).unwrap(),
                    rty: 2,
                    ptr: Box::into_raw(utf_16.into_boxed_slice()) as *mut c_void,
                }
            })
            .collect();
        Self {
            len: i32::try_from(val.len()).unwrap(),
            rty: 0,
            ptr: Box::into_raw(val.into_boxed_slice()) as *mut c_void,
        }
    }

    pub fn into_str_arr(self) -> Vec<String> {
        unsafe {
            (&*slice_from_raw_parts(self.ptr as *mut Self, self.len as usize))
                .into_iter()
                .map(|i| {
                    String::from_utf16(&*slice_from_raw_parts(i.ptr as *mut u16, i.len as usize))
                        .unwrap()
                })
                .collect()
        }
    }

    // RefArr{len, RefArr{len, ptr}}
    #[inline(always)]
    pub fn from_num_arr<T: Default + Clone>(val: Vec<Vec<T>>) -> Self {
        let val: Vec<Self> = val
            .into_iter()
            .map(|i| Self {
                len: i32::try_from(i.len()).unwrap(),
                rty: i32::try_from(size_of::<T>()).unwrap(),
                ptr: Box::into_raw(i.into_boxed_slice()) as *mut c_void,
            })
            .collect();
        Self {
            len: i32::try_from(val.len()).unwrap(),
            rty: 0,
            ptr: Box::into_raw(val.into_boxed_slice()) as *mut c_void,
        }
    }

    #[inline(always)]
    pub fn into_num_arr<T: Default + Clone>(self) -> Vec<Vec<T>> {
        unsafe {
            (&*slice_from_raw_parts(self.ptr as *mut Self, self.len as usize))
                .into_iter()
                .map(|i| (*slice_from_raw_parts(i.ptr as *mut T, i.len as usize)).to_vec())
                .collect()
        }
    }

    #[no_mangle]
    pub unsafe extern "C-unwind" fn RefObjDispose(self) {
        if self.ptr.is_null() || self.len == 0 {
            return;
        }
        match self.rty {
            0 => {
                for this in Box::from_raw(slice_from_raw_parts_mut(
                    self.ptr as *mut RefObj,
                    self.len as usize,
                )) {
                    Self::RefObjDispose(this)
                }
            }
            1 => drop(Box::from_raw(slice_from_raw_parts_mut(
                self.ptr as *mut u8,
                self.len as usize,
            ))),
            2 => drop(Box::from_raw(slice_from_raw_parts_mut(
                self.ptr as *mut u16,
                self.len as usize,
            ))),
            4 => drop(Box::from_raw(slice_from_raw_parts_mut(
                self.ptr as *mut u32,
                self.len as usize,
            ))),
            8 => drop(Box::from_raw(slice_from_raw_parts_mut(
                self.ptr as *mut u64,
                self.len as usize,
            ))),
            _ => return,
        }
    }
}

#[cfg(test)]
mod test {
    use super::*;
    #[test]
    fn iterators_test() {
        let mut value = Iterators::new(b"2024 11 11 18 48 23");
        while let Some(i) = value.next_number() {
            print!("{}\t", i);
        }
        println!();
        let mut value = Iterators::new(b"+-*/");
        while let Some(i) = value.next_number() {
            print!("{}\t", i);
        }
        println!();
        let mut value = Iterators::new(b"");
        while let Some(i) = value.next_number() {
            print!("{}\t", i);
        }
        println!();
        let mut value = Iterators::new("12:00:00-23:30:00".as_bytes());
        while let Some(i) = value.next_number() {
            print!("{}\t", i);
        }
    }
}
