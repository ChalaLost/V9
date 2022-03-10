using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9Common;
using V9ManagerIVR.Models.CRM;
using V9ManagerIVR.Models.Entities;
using V9ManagerIVR.Provider;
using V9ManagerIVR.Services;

namespace V9ManagerIVR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IVRController : V9Controller
    {

        private readonly ILogger<IVRController> _logger;
        private readonly IIVRServices _IVRServices;
        private readonly IScriptServices _ScriptServices;

        public IVRController(ILogger<IVRController> logger, IIVRServices IVRServices, IScriptServices ScriptServices)
        {
            _ScriptServices = ScriptServices;
            _logger = logger;
            _IVRServices = IVRServices;
        }

        /// <summary>
        /// Tạo mới kịch bản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("CreateScript")]
        public async Task<IActionResult> CreateScript([FromBody] IVR model)
        {
            try
            {
                model.CompanyId = Company;
                return Ok(await _ScriptServices.CreateScript(model, Username));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }

        }

        /// <summary>
        /// Xóa kịch bản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete("{ivrId}")]
        public async Task<IActionResult> DeleteScript(Guid ivrId)
        {
            await _ScriptServices.DeleteScript(Company, ivrId, Username);
            return Ok();
        }

        /// <summary>
        /// Cập nhật kịch bản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UpdateScript")]
        public async Task<IActionResult> UpdateScript([FromBody] IVR model)
        {
            await _ScriptServices.UpdateScript(model, Company, Username);
            return Ok();
        }

        /// <summary>
        /// Tất cả kịch bản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("GetScript")]
        public async Task<IActionResult> GetScript()
        {
            return Ok(await _ScriptServices.GetScriptByCompany(Company));
        }


        /// <summary>
        /// Tạo mới IVR Lv3, 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("CreateLV3")]
        public async Task<IActionResult> CreateLV3([FromBody] CreateIVRModel model)
        {
            await Task.CompletedTask;
            return Ok();
        }

        /// <summary>
        /// Tạo mới IVR press key
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("CreatePressKey")]
        public async Task<IActionResult> CreatePressKey([FromBody] CreateIVRModel model)
        {
            await Task.CompletedTask;
            return Ok();
        }

        /// <summary>
        /// Lấy IVR cùng loại
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("GetScriptByAction/{type}")]
        public async Task<IActionResult> GetScriptByAction(string type, Guid? currentIVRId)
        {
            List<ActionType> types = type.Split(',').Select(s => (ActionType)Convert.ToInt32(s)).ToList();
            return Ok(await _ScriptServices.GetScriptByAction(Company, types, currentIVRId));
        }


        /// <summary>
        /// Tất cả theo công ty
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("All/{company}")]
        //public async Task<IActionResult> All(Guid company)
        //{
        //    return Ok(await _IVRServices.All(company));
        //}

        /// <summary>
        /// Bản ghi theo Id
        /// </summary>
        /// <param name="IVRId"></param>
        /// <returns></returns>
        [HttpGet("GetById/{IVRId}")]
        public async Task<IActionResult> GetById(Guid IVRId)
        {
            var item = await _IVRServices.GetById(IVRId);
            return Ok(item);
        }




        //[HttpPost]
        //[Route("TestTaoIVR")]
        //public async Task<IActionResult> CreatedLV3()
        //{
        //    //DateTime.TryParseExact("28/06/2021", FormatDate.DateTime_103, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date1);
        //    //DateTime.TryParseExact("29/06/2021", FormatDate.DateTime_103, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date2);


        //    CreateIVRModel ivr = new()
        //    {
        //        Name = "Ngày thường",
        //        Company = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
        //        Level = V9_IVRLevel.LV3,
        //        Exten = "19000",
        //        IsActive = true,
        //        UserName = "Binhnc",
        //        Priority = 1,
        //        Schedule = new CreateIVRScheduleModel()
        //        {
        //            Type = V9_ScheduleActionType.Date,
        //            ScheduleDates = new List<CreateScheduleDateType>()
        //            {
        //                new CreateScheduleDateType()
        //                {
        //                    DateStr = "28/06/2021",
        //                    Times = new List<CreateScheduleDateTypeTime>()
        //                    {
        //                        new CreateScheduleDateTypeTime()
        //                        {
        //                            FromTimeStr = "08:00",
        //                            ToTimeStr = "12:00"
        //                        },
        //                        new CreateScheduleDateTypeTime()
        //                        {
        //                            FromTimeStr = "14:00",
        //                            ToTimeStr = "17:00"
        //                        }
        //                    }
        //                },
        //                new CreateScheduleDateType()
        //                {
        //                    DateStr = "29/06/2021",
        //                    Times = new List<CreateScheduleDateTypeTime>()
        //                    {
        //                        new CreateScheduleDateTypeTime()
        //                        {
        //                            FromTimeStr = "08:00",
        //                            ToTimeStr = "12:00"
        //                        },
        //                        new CreateScheduleDateTypeTime()
        //                        {
        //                            FromTimeStr = "14:00",
        //                            ToTimeStr = "17:00"
        //                        }
        //                    }
        //                }
        //            }
        //        },
        //        Action = new CreateIVRActionModel()
        //        {
        //            Id = Guid.NewGuid(),
        //            A1_RecordingFile = "mywelcome",
        //            A1_IsPlayMusicWaitting = false,
        //            A1_PlayTimes = 1,
        //            A1_PlayWaittingTime = 2,
        //            A8_HasConfig = true,
        //            Code = V9_IVRAction.ThongBao,
        //            Childrens = new List<CreateIVRActionModel>()
        //            {
        //                new CreateIVRActionModel()
        //                {
        //                    Id = Guid.NewGuid(),
        //                    KeyPress = "2",
        //                    Code = V9_IVRAction.ThongBao,
        //                    A1_RecordingFile = "MainIVRen",
        //                    A1_IsPlayMusicWaitting = false,
        //                    A1_PlayTimes = 2,
        //                    A1_PlayWaittingTime = 4,
        //                    Childrens = new List<CreateIVRActionModel>()
        //                    {
        //                        new CreateIVRActionModel()
        //                        {
        //                            Id = Guid.NewGuid(),
        //                            KeyPress = "2",
        //                            Code = V9_IVRAction.DinhTuyenKhachHang,
        //                            A2_MusicNotifyFile = "TransferToAgent",
        //                            A2_MusicBusyTimes = 2,
        //                            A2_MusicByeFile = "ThankAndBye",
        //                            A2_MusicWaittingTime = 10,
        //                            A2_MusicWaitFile ="Busy1",
        //                            A2_IsQueue = true,
        //                            A2_TransName = "envang",
        //                            A2_MusicBusyFile = "Busy2"
        //                        },
        //                        new CreateIVRActionModel()
        //                        {
        //                            Id = Guid.NewGuid(),
        //                            KeyPress = "3",
        //                            Code = V9_IVRAction.ChuyenTiep,
        //                            A2_MusicNotifyFile = "TransferToAgent",
        //                            A2_MusicBusyTimes = 2,
        //                            A2_MusicByeFile = "ThankAndBye",
        //                            A2_MusicWaittingTime = 15,
        //                            A2_MusicWaitFile ="Busy1",
        //                            A2_IsQueue = false,
        //                            A2_TransName = "3002",
        //                            A2_MusicBusyFile = "Busy2"
        //                        },
        //                        new CreateIVRActionModel()
        //                        {
        //                            Id = Guid.NewGuid(),
        //                            KeyPress = "4",
        //                            Code = V9_IVRAction.ChuyenTiep,
        //                            A2_MusicNotifyFile = "TransferToAgent",
        //                            A2_MusicBusyTimes = 3,
        //                            A2_MusicByeFile = "ThankAndBye",
        //                            A2_MusicWaittingTime = 10,
        //                            A2_MusicWaitFile ="Busy1",
        //                            A2_IsQueue = true,
        //                            A2_TransName = "envang",
        //                            A2_MusicBusyFile = "Busy2"
        //                        },
        //                        new CreateIVRActionModel()
        //                        {
        //                            Id = Guid.NewGuid(),
        //                            KeyPress = "#*0156789",
        //                            Code = V9_IVRAction.NhapSaiPhim,
        //                            A8_HasConfig = true,
        //                            A8_WarningFile = "BanDaNhapSaiPhim",
        //                            A8_WarningTimes = 3,
        //                            NextAction = new CreateForwardActionModel()
        //                            {
        //                                Id = Guid.NewGuid(),
        //                                Code = V9_IVRAction.KetThuc,
        //                                A9_NotificationFile = "KetThucCuocGoi"
        //                            }
        //                        }
        //                    },
        //                    NextAction = new CreateForwardActionModel()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        Code = V9_IVRAction.ChuyenTiep,
        //                        A2_IsQueue = true,
        //                        A2_TransName = "envang",
        //                        A2_MusicBusyFile = "busy1"
        //                    }
        //                },
        //                new CreateIVRActionModel()
        //                {
        //                    Id = Guid.NewGuid(),
        //                    KeyPress = "#*056789",
        //                    Code = V9_IVRAction.NhapSaiPhim,
        //                    A8_HasConfig = true,
        //                    A8_WarningFile = "BanDaNhapSaiPhim",
        //                    A8_WarningTimes = 3,
        //                    NextAction = new CreateForwardActionModel()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        Code = V9_IVRAction.KetThuc,
        //                        A9_NotificationFile = "KetThucCuocGoi"
        //                    }
        //                }
        //            },
        //            NextAction = new CreateForwardActionModel()
        //            {
        //                Id = Guid.NewGuid(),
        //                Code = V9_IVRAction.ThongBao,
        //                A1_RecordingFile = "MainIVR",
        //                A1_IsPlayMusicWaitting = false,
        //                A1_PlayTimes = 2,
        //                A1_PlayWaittingTime = 4,
        //                A1_PressKeyActions = new List<CreateIVRActionModel>()
        //                {
        //                    new CreateIVRActionModel()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        KeyPress = "2",
        //                        Code = V9_IVRAction.DinhTuyenKhachHang,
        //                        A2_MusicNotifyFile = "TransferToAgent",
        //                        A2_MusicBusyTimes = 2,
        //                        A2_MusicByeFile = "ThankAndBye",
        //                        A2_MusicWaittingTime = 10,
        //                        A2_MusicWaitFile ="Busy1",
        //                        A2_IsQueue = true,
        //                        A2_TransName = "envang",
        //                        A2_MusicBusyFile = "Busy2"
        //                    },
        //                    new CreateIVRActionModel()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        KeyPress = "3",
        //                        Code = V9_IVRAction.ChuyenTiep,
        //                        A2_MusicNotifyFile = "TransferToAgent",
        //                        A2_MusicBusyTimes = 2,
        //                        A2_MusicByeFile = "ThankAndBye",
        //                        A2_MusicWaittingTime = 15,
        //                        A2_MusicWaitFile ="Busy1",
        //                        A2_IsQueue = false,
        //                        A2_TransName = "3002",
        //                        A2_MusicBusyFile = "Busy2"
        //                    },
        //                    new CreateIVRActionModel()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        KeyPress = "4",
        //                        Code = V9_IVRAction.ChuyenTiep,
        //                        A2_MusicNotifyFile = "TransferToAgent",
        //                        A2_MusicBusyTimes = 3,
        //                        A2_MusicByeFile = "ThankAndBye",
        //                        A2_MusicWaittingTime = 10,
        //                        A2_MusicWaitFile ="Busy1",
        //                        A2_IsQueue = true,
        //                        A2_TransName = "envang",
        //                        A2_MusicBusyFile = "Busy2"
        //                    },
        //                    new CreateIVRActionModel()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        KeyPress = "#*056789",
        //                        Code = V9_IVRAction.NhapSaiPhim,
        //                        A8_HasConfig = true,
        //                        A8_WarningFile = "BanDaNhapSaiPhim",
        //                        A8_WarningTimes = 3,
        //                        NextAction = new CreateForwardActionModel()
        //                        {
        //                            Id = Guid.NewGuid(),
        //                            Code = V9_IVRAction.KetThuc,
        //                            A9_NotificationFile = "KetThucCuocGoi"
        //                        }
        //                    }
        //                },
        //                NextAction = new CreateForwardActionModel()
        //                {
        //                    Id = Guid.NewGuid(),
        //                    Code = V9_IVRAction.KetThuc,
        //                    A9_NotificationFile = "KetThucCuocGoi"
        //                }
        //            }
        //        }
        //    };

        //    List<ScheduleDateType> _scheduleDates = new();
        //    ScheduleDayOfWeek _scheduleDayOfWeek = new(); ;

        //    if (ivr.Schedule.Type == V9_ScheduleActionType.Date)
        //    {
        //        _scheduleDates = ivr.Schedule.ScheduleDates.Select(s => new ScheduleDateType()
        //        {
        //            Id = Guid.NewGuid(),
        //            Date = s.Date.Value,
        //            Times = s.Times.Select(t => new ScheduleDateTypeTime()
        //            {
        //                Id = Guid.NewGuid(),
        //                FromTime = t.FromTime.Value,
        //                ToTime = t.ToTime.Value
        //            }).ToList()
        //        }).ToList();
        //    }
        //    else
        //    {
        //        _scheduleDayOfWeek = new ScheduleDayOfWeek()
        //        {
        //            Id = Guid.NewGuid(),
        //            DayOfWeeks = ivr.Schedule.ScheduleDayOfWeek.DayOfWeeks.Select(s => new Models.Entities.DayOfWeeks()
        //            {
        //                Id = Guid.NewGuid(),
        //                Value = s.Value,
        //                Times = s.Times.Select(s => new DayOfWeekTimes()
        //                {
        //                    Id = Guid.NewGuid(),
        //                    FromTime = s.FromTime.Value,
        //                    ToTime = s.ToTime.Value
        //                }).ToList()
        //            }).ToList(),
        //            Months = ivr.Schedule.ScheduleDayOfWeek.Months.Select(s => new Month()
        //            {
        //                Id = Guid.NewGuid(),
        //                Value = s.Value
        //            }).ToList()
        //        };
        //    }

        //    await _Context.AddAsync(new IVR()
        //    {
        //        Id = Guid.NewGuid(),
        //        Company = ivr.Company,
        //        Extens = new List<IVRExten>()
        //        {
        //            new IVRExten()
        //            {
        //                Id = Guid.NewGuid(),
        //                Exten = "19000",
        //                Provider = "VDC"
        //            },
        //            new IVRExten()
        //            {
        //                Id = Guid.NewGuid(),
        //                Exten = "19001800",
        //                Provider = "JCS"
        //            }
        //        },
        //        Name = ivr.Name,
        //        Level = ivr.Level,
        //        Priority = ivr.Priority,
        //        IsDeleted = false,
        //        IsActive = ivr.IsActive,
        //        CreatedBy = ivr.UserName,
        //        CreatedDate = DateTime.Now,
        //        Schedule = new Schedule()
        //        {
        //            Id = Guid.NewGuid(),
        //            Type = ivr.Schedule.Type,
        //            ScheduleDates = _scheduleDates,
        //            ScheduleDayOfWeek = _scheduleDayOfWeek
        //        },
        //        Action = new Models.Entities.Action()
        //        {
        //            Id = Guid.NewGuid(),
        //            A1_RecordingFile = "mywelcome",
        //            A1_IsPlayMusicWaitting = false,
        //            A1_PlayTimes = 1,
        //            A1_PlayWaittingTime = 2,
        //            A8_HasConfig = true,
        //            Code = V9_IVRAction.ThongBao,
        //            Childrens = new List<Models.Entities.Action>()
        //            {
        //                new Models.Entities.Action()
        //                {
        //                    Id = Guid.NewGuid(),
        //                    KeyPress = "2",
        //                    Code = V9_IVRAction.ThongBao,
        //                    A1_RecordingFile = "MainIVRen",
        //                    A1_IsPlayMusicWaitting = false,
        //                    A1_PlayTimes = 2,
        //                    A1_PlayWaittingTime = 4,
        //                    Childrens = new List<Models.Entities.Action>()
        //                    {
        //                        new Models.Entities.Action()
        //                        {
        //                            Id = Guid.NewGuid(),
        //                            KeyPress = "2",
        //                            Code = V9_IVRAction.DinhTuyenKhachHang,
        //                            A2_MusicNotifyFile = "TransferToAgent",
        //                            A2_MusicBusyTimes = 2,
        //                            A2_MusicByeFile = "ThankAndBye",
        //                            A2_MusicWaittingTime = 10,
        //                            A2_MusicWaitFile ="Busy1",
        //                            A2_IsQueue = true,
        //                            A2_TransName = "envang",
        //                            A2_MusicBusyFile = "Busy2"
        //                        },
        //                        new Models.Entities.Action()
        //                        {
        //                            Id = Guid.NewGuid(),
        //                            KeyPress = "3",
        //                            Code = V9_IVRAction.ChuyenTiep,
        //                            A2_MusicNotifyFile = "TransferToAgent",
        //                            A2_MusicBusyTimes = 2,
        //                            A2_MusicByeFile = "ThankAndBye",
        //                            A2_MusicWaittingTime = 15,
        //                            A2_MusicWaitFile ="Busy1",
        //                            A2_IsQueue = false,
        //                            A2_TransName = "3002",
        //                            A2_MusicBusyFile = "Busy2"
        //                        },
        //                        new Models.Entities.Action()
        //                        {
        //                            Id = Guid.NewGuid(),
        //                            KeyPress = "4",
        //                            Code = V9_IVRAction.ChuyenTiep,
        //                            A2_MusicNotifyFile = "TransferToAgent",
        //                            A2_MusicBusyTimes = 3,
        //                            A2_MusicByeFile = "ThankAndBye",
        //                            A2_MusicWaittingTime = 10,
        //                            A2_MusicWaitFile ="Busy1",
        //                            A2_IsQueue = true,
        //                            A2_TransName = "envang",
        //                            A2_MusicBusyFile = "Busy2"
        //                        },
        //                        new Models.Entities.Action()
        //                        {
        //                            Id = Guid.NewGuid(),
        //                            KeyPress = "#*0156789",
        //                            Code = V9_IVRAction.NhapSaiPhim,
        //                            A8_HasConfig = true,
        //                            A8_WarningFile = "BanDaNhapSaiPhim",
        //                            A8_WarningTimes = 3,
        //                            NextAction = new ForwardAction()
        //                            {
        //                                Id = Guid.NewGuid(),
        //                                Code = V9_IVRAction.KetThuc,
        //                                A9_NotificationFile = "KetThucCuocGoi"
        //                            }
        //                        }
        //                    },
        //                    NextAction = new ForwardAction()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        Code = V9_IVRAction.ChuyenTiep,
        //                        A2_IsQueue = true,
        //                        A2_TransName = "envang",
        //                        A2_MusicBusyFile = "busy1"
        //                    }
        //                },
        //                new Models.Entities.Action()
        //                {
        //                    Id = Guid.NewGuid(),
        //                    KeyPress = "#*056789",
        //                    Code = V9_IVRAction.NhapSaiPhim,
        //                    A8_HasConfig = true,
        //                    A8_WarningFile = "BanDaNhapSaiPhim",
        //                    A8_WarningTimes = 3,
        //                    NextAction = new ForwardAction()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        Code = V9_IVRAction.KetThuc,
        //                        A9_NotificationFile = "KetThucCuocGoi"
        //                    }
        //                }
        //            },
        //            NextAction = new ForwardAction()
        //            {
        //                Id = Guid.NewGuid(),
        //                Code = V9_IVRAction.ThongBao,
        //                A1_RecordingFile = "MainIVR",
        //                A1_IsPlayMusicWaitting = false,
        //                A1_PlayTimes = 2,
        //                A1_PlayWaittingTime = 4,
        //                A1_KeyPressActions = new List<Models.Entities.Action>()
        //                {
        //                    new Models.Entities.Action()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        KeyPress = "2",
        //                        Code = V9_IVRAction.DinhTuyenKhachHang,
        //                        A2_MusicNotifyFile = "TransferToAgent",
        //                        A2_MusicBusyTimes = 2,
        //                        A2_MusicByeFile = "ThankAndBye",
        //                        A2_MusicWaittingTime = 10,
        //                        A2_MusicWaitFile ="Busy1",
        //                        A2_IsQueue = true,
        //                        A2_TransName = "envang",
        //                        A2_MusicBusyFile = "Busy2"
        //                    },
        //                    new Models.Entities.Action()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        KeyPress = "3",
        //                        Code = V9_IVRAction.ChuyenTiep,
        //                        A2_MusicNotifyFile = "TransferToAgent",
        //                        A2_MusicBusyTimes = 2,
        //                        A2_MusicByeFile = "ThankAndBye",
        //                        A2_MusicWaittingTime = 15,
        //                        A2_MusicWaitFile ="Busy1",
        //                        A2_IsQueue = false,
        //                        A2_TransName = "3002",
        //                        A2_MusicBusyFile = "Busy2"
        //                    },
        //                    new Models.Entities.Action()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        KeyPress = "4",
        //                        Code = V9_IVRAction.ChuyenTiep,
        //                        A2_MusicNotifyFile = "TransferToAgent",
        //                        A2_MusicBusyTimes = 3,
        //                        A2_MusicByeFile = "ThankAndBye",
        //                        A2_MusicWaittingTime = 10,
        //                        A2_MusicWaitFile ="Busy1",
        //                        A2_IsQueue = true,
        //                        A2_TransName = "envang",
        //                        A2_MusicBusyFile = "Busy2"
        //                    },
        //                    new Models.Entities.Action()
        //                    {
        //                        Id = Guid.NewGuid(),
        //                        KeyPress = "#*056789",
        //                        Code = V9_IVRAction.NhapSaiPhim,
        //                        A8_HasConfig = true,
        //                        A8_WarningFile = "BanDaNhapSaiPhim",
        //                        A8_WarningTimes = 3,
        //                        NextAction = new ForwardAction()
        //                        {
        //                            Id = Guid.NewGuid(),
        //                            Code = V9_IVRAction.KetThuc,
        //                            A9_NotificationFile = "KetThucCuocGoi"
        //                        }
        //                    }
        //                },
        //                NextAction = new ForwardAction()
        //                {
        //                    Id = Guid.NewGuid(),
        //                    Code = V9_IVRAction.KetThuc,
        //                    A9_NotificationFile = "KetThucCuocGoi"
        //                }
        //            }
        //        }
        //    });

        //    await _Context.SaveChangesAsync();

        //    await Task.CompletedTask;

        //    return Ok(JsonConvert.SerializeObject(ivr));
        //}
    }
}
