﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotNETCore.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        public string SamAccountName { get; set; }
        public string Manager { get; set; }
        public int Pid { get; set; }
        public string Image { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Disabled { get; set; }
    }
}

