using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFinder.ViewModels
{
    public class Region
    {
        public string name { get; set; }
        public string name_ua { get; set; }
        public string slug { get; set; }
        public string old_code { get; set; }
        public string new_code { get; set; }
    }

    public class Body
    {
        public int id { get; set; }
        public string ua { get; set; }
        public string ru { get; set; }
    }

    public class Purpose
    {
        public int id { get; set; }
        public string ua { get; set; }
        public string ru { get; set; }
    }

    public class Operation
    {
        public string ru { get; set; }
        public string ua { get; set; }
    }

    public class Color
    {
        public string slug { get; set; }
        public string ru { get; set; }
        public string ua { get; set; }
    }

    public class Kind
    {
        public int id { get; set; }
        public string ru { get; set; }
        public string ua { get; set; }
        public string slug { get; set; }
    }

    public class OperationGroup
    {
        public int id { get; set; }
        public string ru { get; set; }
        public string ua { get; set; }
    }

    public class OperationDetail
    {
        public bool is_last { get; set; }
        public string catalog_model_title { get; set; }
        public string catalog_model_slug { get; set; }
        public Body body { get; set; }
        public Purpose purpose { get; set; }
        public string registered_at { get; set; }
        public int model_year { get; set; }
        public string vendor { get; set; }
        public string vendor_slug { get; set; }
        public string model { get; set; }
        public Operation operation { get; set; }
        public string department { get; set; }
        public Color color { get; set; }
        public bool is_registered_to_company { get; set; }
        public string address { get; set; }
        public long koatuu { get; set; }
        public int displacement { get; set; }
        public Kind kind { get; set; }
        public OperationGroup operation_group { get; set; }
    }

    public class Comment
    {
        public int id { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class CarInfo
    {
        public string digits { get; set; }
        public string vin { get; set; }
        public Region region { get; set; }
        public string vendor { get; set; }
        public string model { get; set; }
        public int model_year { get; set; }
        public string photo_url { get; set; }
        public bool is_stolen { get; set; }
        public object stolen_details { get; set; }
        public List<OperationDetail> operations { get; set; }
        public List<Comment> comments { get; set; }
    }
}
