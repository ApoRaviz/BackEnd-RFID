using HRMS.Common.ValueObject.LeaveManagement;
using HRMS.Entity.LeaveManagement;
using System.Collections.Generic;
using WIM.Core.Service;

namespace HRMS.Service.LeaveManagement
{
    public interface ILeaveService : IService
    {
        /// <summary>
        /// ดึงข้อมูลออกมา 1 แถวโดย Where ที่ LeaveIDSys
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LeaveDto GetLeaveByID(int id);
        /// <summary>
        /// ดึงข้อมูลในตาราง Leaves ออกมาเป็น List
        /// </summary>
        /// <returns></returns>
        IEnumerable<Leave> GetLeaves();
        /// <summary>
        /// ใช้ตอนสร้าง Request การลา  โดยมีการส่ง Detail มา Save ด้วย
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        Leave CreateLeave(Leave leave);
        /// <summary>
        /// ใช้ตอนแก้ไขข้อมูล Request การลา  ในขณะที่สถานะยังเป็น Request อยู่
        /// </summary>
        /// <param name="leave"></param>
        /// <returns></returns>
        Leave UpdateLeave(LeaveDto leave);
        IEnumerable<LeaveTypeDto> GetLeaveType();

        bool ApproveLeave(int id);
        bool RejectLeave(int id);

    }
}
