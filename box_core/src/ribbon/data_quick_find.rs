use crate::{ptr_is_null, RefObj};
use std::{
    collections::BTreeMap,
    ffi::c_void,
    ptr::{null, slice_from_raw_parts},
    sync::RwLock,
};

struct DataQuickFind;

impl DataQuickFind {
    #[no_mangle]
    pub unsafe extern "C-unwind" fn DQFCreate() -> *mut RwLock<BTreeMap<Vec<u16>, Vec<i32>>> {
        Box::into_raw(Box::new(RwLock::new(BTreeMap::new())))
    }

    #[no_mangle]
    pub unsafe extern "C-unwind" fn DQFWrite(
        bmap: *mut RwLock<BTreeMap<Vec<u16>, Vec<i32>>>,
        key: RefObj,
        idx: i32,
    ) {
        ptr_is_null!(bmap);
        let key = (&*slice_from_raw_parts(key.ptr as *mut u16, key.len as usize)).to_vec();
        let mut map = (*bmap).write().unwrap();
        if let Some(v) = map.get_mut(&key) {
            v.push(idx);
        } else {
            map.insert(key, {
                let mut value = Vec::with_capacity(64);
                value.push(idx);
                value
            });
        }
    }

    #[no_mangle]
    pub unsafe extern "C-unwind" fn DQFFinds(
        bmap: *mut RwLock<BTreeMap<Vec<u16>, Vec<i32>>>,
        key: RefObj,
    ) -> RefObj {
        ptr_is_null!(bmap);
        let key = (&*slice_from_raw_parts(key.ptr as *mut u16, key.len as usize)).to_vec();
        if let Some(val) = (*bmap).read().unwrap().get(&key) {
            RefObj {
                len: i32::try_from(val.len()).unwrap(),
                rty: 4,
                ptr: Box::into_raw(val.clone().into_boxed_slice()) as *mut c_void,
            }
        } else {
            RefObj {
                len: 0,
                rty: 4,
                ptr: null::<u16>() as *mut c_void,
            }
        }
    }

    #[no_mangle]
    pub unsafe extern "C-unwind" fn DQFKeys(
        bmap: *mut RwLock<BTreeMap<Vec<u16>, Vec<i32>>>,
    ) -> RefObj {
        ptr_is_null!(bmap);
        let keys = (*bmap)
            .read()
            .unwrap()
            .keys()
            .map(|key| RefObj {
                len: i32::try_from(key.len()).unwrap(),
                rty: 2,
                ptr: Box::into_raw(key.clone().into_boxed_slice()) as *mut c_void,
            })
            .collect::<Vec<RefObj>>();
        RefObj {
            len: i32::try_from(keys.len()).unwrap(),
            rty: 0,
            ptr: Box::into_raw(keys.into_boxed_slice()) as *mut c_void,
        }
    }

    #[no_mangle]
    pub unsafe extern "C-unwind" fn DQFDispose(bmap: *mut RwLock<BTreeMap<Vec<u16>, Vec<i32>>>) {
        ptr_is_null!(bmap);
        drop(Box::from_raw(bmap))
    }
}
