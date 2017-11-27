using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Entity;

namespace TMS.Entity
{
    [Table("PreBooking")]
    public class PreBooking : BaseEntity
    {
        [Key]
        public int IDSys { get; set; }
        public string ref_id { get; set; }
        public string ref_customer { get; set; }
        public string cus_id { get; set; }
        public string cus_name { get; set; }
        public int transport_mode_id { get; set; }
        public int vehicle_type_id { get; set; }
        public int truck_qty { get; set; }
        public int manpower { get; set; }
        public string revenue { get; set; }
        public string net_value { get; set; }
        public string revenue_fwd { get; set; }
        public int revenue_type { get; set; }
        public string hawb { get; set; }
        public string mawb { get; set; }
        public string lot_number { get; set; }
        public DateTime receive_date { get; set; }
        public int receive_time_show { get; set; }
        public int receive_landmark_id { get; set; }
        public string receive_company_name { get; set; }
        public string receive_address { get; set; }
        public string receive_postalcode { get; set; }
        public string receive_contact { get; set; }
        public string receive_phone { get; set; }
        public string receive_latitude { get; set; }
        public string receive_longitude { get; set; }
        public DateTime send_date { get; set; }
        public int send_time_show { get; set; }
        public int send_landmark_id { get; set; }
        public string send_company_name { get; set; }
        public string send_address { get; set; }
        public string send_postalcode { get; set; }
        public string send_contact { get; set; }
        public string send_phone { get; set; }
        public string send_latitude { get; set; }
        public string send_longitude { get; set; }
        public string remark { get; set; }
        public string remark_shipper { get; set; }
        public int total_pcks { get; set; }
        public Decimal c_weight { get; set; }
        public Decimal total_weight { get; set; }
        public float cbm { get; set; }
        public string booking_status { get; set; }
        public string user_id { get; set; }
        public DateTime date_create { get; set; }
        public int edit_mode { get; set; }
        public DateTime time_stamp { get; set; }
        public int print_detail { get; set; }
        public int print_slip { get; set; }
        public string tr_work_type { get; set; }

    }
}
