﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CareConnect.Models.CareConnectViewModels
{
    public class DepartmentViewModel
    {
        public int DepartmentId { get; set; }
        [ForeignKey("Department_Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
