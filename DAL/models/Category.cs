using System.ComponentModel.DataAnnotations.Schema;
using DAl.models;

namespace DAL.models
{


    [Table("Categories", Schema = "MasterSchema")]
    public class Category
    {
        //[Key]
        public int Id { get; set; }

        //[Required]
        //[MaxLength(50)]
        public string catName { get; set; }

        //[Required]
        public int catOrder { get; set; }

        //[NotMapped]
        public DateTime createdDate { get; set; } = DateTime.Now;

        //[Column("is deleted")]
        public bool markedAsDeleted { get; set; }

        public ICollection<Product> products { get; set; }
    }
}