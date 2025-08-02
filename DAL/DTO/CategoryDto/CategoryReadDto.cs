using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.CategoryDto
{
    public class CategoryReadDto
    {
        public int Id { get; set; }
        public string catName { get; set; }
        public int catOrder { get; set; }
        public DateTime createdDate { get; set; }
        public bool markedAsDeleted { get; set; }


    }
}
