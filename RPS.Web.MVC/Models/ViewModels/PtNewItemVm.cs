using Microsoft.AspNetCore.Mvc.Rendering;
using RPS.Core.Models.Dto;
using RPS.Core.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RPS.Web.Models.ViewModels
{
    public class PtNewItemVm
    {
        private readonly List<ItemTypeEnum> _itemTypes = new List<ItemTypeEnum> { ItemTypeEnum.Bug, ItemTypeEnum.Chore, ItemTypeEnum.Impediment, ItemTypeEnum.PBI };


        [Required, Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Type")]
        public ItemTypeEnum TypeStr { get; set; }

        public IEnumerable<SelectListItem> ItemTypes 
        {
            get { return new SelectList(_itemTypes, ItemTypeEnum.Bug); }
        }

        public PtNewItemVm()
        {
            TypeStr = ItemTypeEnum.Bug;
        }

        public PtNewItem ToPtNewItem()
        {
            return new PtNewItem
            {
                Title = Title,
                Description = Description,
                TypeStr = TypeStr
            };
        }
    }
}