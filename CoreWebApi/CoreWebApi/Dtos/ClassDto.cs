﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class ClassDto
    {
        
    }
    public class ClassDtoForAdd : BaseDto
    {
        [Required]
        [StringLength(30, ErrorMessage = "Class Name cannot be longer than 30 characters.")]
        public string Name { get; set; }

    }
    public class ClassDtoForEdit : BaseDto
    {
        [Required]
        [StringLength(30, ErrorMessage = "Class Name cannot be longer than 30 characters.")]
        public string Name { get; set; }
        public bool Active { get; set; }

    }
    public class ClassDtoForList
    {
    }
    public class ClassDtoForDetail
    {
    }
    public class ClassSectionDtoForAdd : BaseDto
    {
        [Required]
        public int SchoolAcademyId { get; set; }
        [Required]
        public int ClassId { get; set; }
        [Required]
        public int SectionId { get; set; }
        public bool Active { get; set; }
    }
    public class ClassSectionDtoForUpdate : BaseDto
    {
        public int Id { get; set; }
        [Required]
        public int SchoolAcademyId { get; set; }
        [Required]
        public int ClassId { get; set; }
        [Required]
        public int SectionId { get; set; }
        public bool Active { get; set; }
    }
    public class ClassSectionUserDtoForAdd
    {
        [Required]
        public int ClassSectionId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
    public class ClassSectionUserDtoForAddBulk
    {
        [Required]
        public int ClassSectionId { get; set; }
        [Required]
        public List<int> UserIds { get; set; }
    }
}
