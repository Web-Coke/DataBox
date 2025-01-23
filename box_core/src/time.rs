use crate::{ptr_is_null, Iterators, RefObj};

const MON_LIST: [[u64; 13]; 2] = [
    [
        0x00, 0x00, 0x1F, 0x3B, 0x5A, 0x78, 0x97, 0xB5, 0xD4, 0xF3, 0x111, 0x130, 0x14E,
    ],
    [
        0x00, 0x00, 0x1F, 0x3C, 0x5B, 0x79, 0x98, 0xB6, 0xD5, 0xF4, 0x112, 0x131, 0x14F,
    ],
];

macro_rules! leap_year {
    ($year:expr) => {
        $year % 0x04 == 0x00 && ($year % 0x64 != 0x00 || $year % 0x0190 == 0x00)
    };
}

#[derive(PartialEq, PartialOrd)]
pub struct TimeStamp {
    ymd: u64,
    hms: u64,
}

impl From<SimpleTime> for TimeStamp {
    fn from(mut value: SimpleTime) -> Self {
        value.day += unsafe {
            *MON_LIST
                .get_unchecked(if leap_year!(value.year) { 0x01 } else { 0x00 })
                .get_unchecked(value.month as usize)
        };
        let rad = value.year & 3;
        if value.year != 0x00 {
            if rad == 0x00 {
                value.year -= 4;
                value.day += 0x5B4;
            } else {
                value.year -= rad;
                value.day += rad * 0x016D;
            }
        } else {
            value.day += rad * 0x016D;
        }
        value.day += (value.year >> 0x02) * 0x05B5;
        TimeStamp {
            ymd: value.day * 0x015180,
            hms: value.hour * 0x0E10 + value.minute * 0x3C + value.second,
        }
    }
}

#[repr(C)]
pub struct SimpleTime {
    year: u64,
    month: u64,
    day: u64,
    hour: u64,
    minute: u64,
    second: u64,
}

impl From<TimeStamp> for SimpleTime {
    fn from(value: TimeStamp) -> Self {
        let (second, minute, hour) = {
            let mut s = value.hms % 0x015180;
            let mut m = s / 0x3C;
            s %= 0x3C;
            let mut h = m / 0x3C;
            m %= 0x3C;
            h %= 0x18;
            (s, m, h)
        };
        let (year, month, day) = {
            let mut y = (value.ymd / 0x07861F80) * 4;
            let mut d = (value.ymd % 0x07861F80) / 0x015180;
            while d > 0x016D {
                d -= 0x016D;
                y += 0x01;
            }
            let mon_list = unsafe {
                MON_LIST.get_unchecked(if leap_year!(y) {
                    d += 1;
                    0x01
                } else {
                    0x00
                })
            };
            let m = match mon_list.binary_search(&d) {
                Ok(m) => m - 1,
                Err(m) => m - 1,
            };
            d -= unsafe { *mon_list.get_unchecked(m) };
            (y, m as u64, d)
        };
        Self {
            year,
            month,
            day,
            hour,
            minute,
            second,
        }
    }
}

impl SimpleTime {
    #[allow(dead_code)]
    fn into_string(self) -> String {
        format!(
            "{:0>4}-{:0>2}-{:0>2} {:0>2}:{:0>2}:{:0>2}",
            self.year, self.month, self.day, self.hour, self.minute, self.second
        )
    }

    // 只支持解析YYYY-MM-DD HH:MM:SS
    fn new(mut time: Iterators) -> Self {
        let mut fun = |i: u64, n: u64| -> u64 {
            let time = time.next_number().unwrap();
            if i <= time && time <= n {
                time
            } else {
                panic!("无效的时间!")
            }
        };
        let year = fun(0x01, 0x270F);
        let month = fun(0x01, 0x0C);
        let day = fun(0x01, unsafe {
            *[
                0x00,
                0x1F,
                if leap_year!(year) { 0x1D } else { 0x1C },
                0x1F,
                0x1E,
                0x1F,
                0x1E,
                0x1F,
                0x1F,
                0x1E,
                0x1F,
                0x1E,
                0x1F,
            ]
            .get_unchecked(month as usize)
        });
        Self {
            year,
            month,
            day,
            hour: fun(0x00, 0x17),
            minute: fun(0x00, 0x3B),
            second: fun(0x00, 0x3B),
        }
    }
}

pub struct Include {
    include_i: u64,
    include_n: u64,
    compute_fn: fn(&Include, TimeStamp, TimeStamp) -> u64,
}

impl Include {
    pub fn new(mut include: Iterators) -> Self {
        let mut fun = |i: u64, n: u64| -> u64 {
            let time = include.next_number().unwrap();
            if i <= time && time <= n {
                time
            } else {
                panic!("无效的时间!")
            }
        };
        let include_i = fun(0x00, 0x17) * 0x0E10 + fun(0x00, 0x3B) * 0x3C + fun(0x00, 0x3B);
        let include_n = fun(0x00, 0x17) * 0x0E10 + fun(0x00, 0x3B) * 0x3C + fun(0x00, 0x3B);
        if include_i <= include_n {
            Self {
                include_i,
                include_n,
                compute_fn: Self::_t,
            }
        } else {
            let (include_i, include_n) = (include_n, include_i);
            Self {
                include_i,
                include_n,
                compute_fn: Self::_n,
            }
        }
    }

    fn __subtraction__(&self, mut time_i: TimeStamp, time_n: TimeStamp) -> u64 {
        let mut mark = 0;
        let time_i_hms = time_i.hms;
        let time_n_hms = time_n.hms;
        if time_i_hms <= time_n_hms {
            if self.include_i <= time_n_hms && time_i_hms <= self.include_n {
                mark += if time_n_hms <= self.include_n {
                    time_n_hms
                } else {
                    self.include_n
                } - if time_i_hms <= self.include_i {
                    self.include_i
                } else {
                    time_i_hms
                }
            }
        } else {
            time_i.ymd += 0x015180;
            if time_i_hms <= self.include_n {
                mark += self.include_n
                    - if time_i_hms <= self.include_i {
                        self.include_i
                    } else {
                        time_i_hms
                    }
            }
            if self.include_i <= time_n_hms {
                mark += if time_n_hms <= self.include_n {
                    time_n_hms
                } else {
                    self.include_n
                } - self.include_i
            }
        }
        mark + ((time_n.ymd - time_i.ymd) / 0x015180) * (self.include_n - self.include_i)
    }

    // to_day 计算在内的时间在当天 如 12:00:00-14:00:00   2小时
    pub fn _t(&self, time_i: TimeStamp, time_n: TimeStamp) -> u64 {
        self.__subtraction__(time_i, time_n)
    }
    // next_day 计算在内的时间在次日 如 14:00:00-12:00:00  22小时
    pub fn _n(&self, time_i: TimeStamp, time_n: TimeStamp) -> u64 {
        ((time_n.ymd + time_n.hms) - (time_i.ymd + time_i.hms))
            - self.__subtraction__(time_i, time_n)
    }
    pub fn subtraction(&self, time_i: TimeStamp, time_n: TimeStamp) -> u64 {
        let (time_i, time_n) = if time_i > time_n {
            (time_n, time_i)
        } else {
            (time_i, time_n)
        };
        (self.compute_fn)(self, time_i, time_n)
    }
}

#[no_mangle]
pub unsafe extern "C-unwind" fn SecToTime(second: i64) -> RefObj {
    if second < 0 {
        panic!("秒数不能小于0")
    }
    let (minute, second) = (second / 0x3C, second % 0x3C);
    let (hour, minute) = (minute / 0x3C, minute % 0x3C);
    RefObj::from_str(format!("{:0>2}:{:0>2}:{:0>2}", hour, minute, second))
}

#[no_mangle]
pub unsafe extern "C-unwind" fn TimeSub(time_i: SimpleTime, time_n: SimpleTime) -> i64 {
    let (time_i, time_n) = (TimeStamp::from(time_i), TimeStamp::from(time_n));
    if time_i > time_n {
        ((time_i.ymd + time_i.hms) - (time_n.ymd + time_n.hms)) as i64
    } else {
        ((time_n.ymd + time_n.hms) - (time_i.ymd + time_i.hms)) as i64
    }
}

#[no_mangle]
pub unsafe extern "C-unwind" fn TimeSubCreate(include: RefObj) -> *mut Include {
    let include = include.into_str();
    Box::into_raw(Box::new(Include::new(Iterators::new(include.as_bytes()))))
}

#[no_mangle]
pub unsafe extern "C-unwind" fn TimeSubCompute(
    time: *mut Include,
    time_i: SimpleTime,
    time_n: SimpleTime,
) -> i64 {
    ptr_is_null!(time);
    let (time_i, time_n) = (TimeStamp::from(time_i), TimeStamp::from(time_n));
    (&*time).subtraction(time_i, time_n) as i64
}

#[no_mangle]
pub unsafe extern "C-unwind" fn TimeSubDispose(time: *mut Include) {
    ptr_is_null!(time);
    drop(Box::from_raw(time))
}

#[no_mangle]
pub unsafe extern "C-unwind" fn ParseTime(time: RefObj) -> SimpleTime {
    let time = time.into_str();
    SimpleTime::new(Iterators::new(time.as_bytes()))
}

#[cfg(test)]
mod test {
    macro_rules! get_test_time {
        ($expr:expr) => {
            let times = std::time::Instant::now();
            $expr
            let end = times.elapsed();
            println!("{:?}", end);
        };
    }
    use super::*;
    use std::panic::catch_unwind;
    #[test]
    fn sec_to_time_test() {
        unsafe {
            let time = SecToTime(27184);
            println!("{}", time.into_str());
        }
    }

    #[test]
    fn time_sub_test() {
        unsafe {
            get_test_time!({
                let time_i = ParseTime(RefObj::from_str("2024/04/08 16:34:52"));
                let time_n = ParseTime(RefObj::from_str("2020/01/12 21:22:36"));
                if 133729936 != TimeSub(time_i, time_n) {
                    panic!()
                }
            });
            get_test_time!({
                let time_i = ParseTime(RefObj::from_str("2023/06/12 17:32:44"));
                let time_n = ParseTime(RefObj::from_str("2023/06/12 08:22:17"));
                if 33027 != TimeSub(time_i, time_n) {
                    panic!()
                }
            });
            get_test_time!({
                let time_i = ParseTime(RefObj::from_str("2024/04/08 21:22:52"));
                let time_n = ParseTime(RefObj::from_str("2024/04/08 16:34:52"));
                if 17280 != TimeSub(time_i, time_n) {
                    panic!()
                }
            });
            get_test_time!({
                let time_i = ParseTime(RefObj::from_str("2023/12/31 00:00:00"));
                let time_n = ParseTime(RefObj::from_str("2024/12/31 00:00:00"));
                if 31622400 != TimeSub(time_i, time_n) {
                    panic!()
                }
            });

            get_test_time!({
                let time_i = ParseTime(RefObj::from_str("2024/12/11 12:00:00"));
                let time_n = ParseTime(RefObj::from_str("2024/12/12 12:00:00"));
                if 86400 != TimeSub(time_i, time_n) {
                    panic!()
                }
            });
        }
    }

    #[test]
    fn time_sub2_test() {
        unsafe {
            get_test_time!({
                let time = TimeSubCompute(
                    TimeSubCreate(RefObj::from_str("22:00:00-02:00:00")),
                    ParseTime(RefObj::from_str("2024 11 11 12 00 00")),
                    ParseTime(RefObj::from_str("2024 11 12 12 00 00")),
                );
                if 14400 != time {
                    panic!("{}", time)
                }
            });

            get_test_time!({
                let time = TimeSubCompute(
                    TimeSubCreate(RefObj::from_str("06:00:00-23:00:00")),
                    ParseTime(RefObj::from_str("2024 11 11 23 48 22")),
                    ParseTime(RefObj::from_str("2024 11 10 23 48 23")),
                );
                if 61200 != time {
                    panic!("{}", time)
                }
            });

            get_test_time!({
                let time = TimeSubCompute(
                    TimeSubCreate(RefObj::from_str("06:00:00-23:00:00")),
                    ParseTime(RefObj::from_str("2024 11 11 23 12 23")),
                    ParseTime(RefObj::from_str("2024 11 11 23 48 23")),
                );
                if 0 != time {
                    panic!("{}", time)
                }
            });

            get_test_time!({
                let time = TimeSubCompute(
                    TimeSubCreate(RefObj::from_str("22:00:00-02:00:00")),
                    ParseTime(RefObj::from_str("2024 11 11 12 00 00")),
                    ParseTime(RefObj::from_str("2024 11 12 12 00 00")),
                );
                if 14400 != time {
                    panic!("{}", time)
                }
            });

            get_test_time!({
                let time = TimeSubCompute(
                    TimeSubCreate(RefObj::from_str("00:00:00-00:00:01")),
                    ParseTime(RefObj::from_str("2023/12/31 00:00:00")),
                    ParseTime(RefObj::from_str("2024/12/31 00:00:00")),
                );
                if 366 != time {
                    panic!("{}", time)
                }
            });

            get_test_time!({
                let time = TimeSubCompute(
                    TimeSubCreate(RefObj::from_str("00:00:01-00:00:00")),
                    ParseTime(RefObj::from_str("2023/12/21 23:59:59")),
                    ParseTime(RefObj::from_str("2023/12/22 00:00:00")),
                );
                if 1 != time {
                    panic!("{}", time)
                }
            });
        }
    }

    #[test]
    fn parse_time_test() {
        get_test_time!({
            unsafe {
                let time = ParseTime(RefObj::from_str("2023/12/31 00:00:00"));
                println!("{}", time.into_string());
            }
        });
        get_test_time!({
            unsafe {
                _ = catch_unwind(|| {
                    let time = ParseTime(RefObj::from_str("2024/12/31 00:00:00"));
                    println!("{}", time.into_string());
                });
            }
        });
        get_test_time!({
            unsafe {
                _ = catch_unwind(|| {
                    let time = ParseTime(RefObj::from_str("2024/06/31 12:00:00"));
                    println!("{}", time.into_string());
                });
            }
        });
    }

    #[test]
    fn time_stamp_to_time_test() {
        let time = SimpleTime::from(TimeStamp::from(SimpleTime::new(Iterators::new(
            b"2023/12/31 00:00:00",
        ))))
        .into_string();
        println!("{}", time);
        let time = SimpleTime::from(TimeStamp::from(SimpleTime::new(Iterators::new(
            b"2024/12/31 00:00:00",
        ))))
        .into_string();
        println!("{}", time);
    }
}
