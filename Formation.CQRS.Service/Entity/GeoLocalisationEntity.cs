using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Formation.CQRS.Service.Entity
{
    [Table("localisation")]
    public class GeoLocalisationEntity
    {
        [Key()]
        [Column("id", TypeName="bigint")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set;}

        [Column("guid", TypeName="character")]
        public String guid { get; set;}

        [Column("date", TypeName="timestamp ")]
        public DateTime date { get; set;}

        [Column("latitude", TypeName="real")]
        public float latitude { get; set;}

        [Column("longitude", TypeName="real")]
        public float longitude { get; set;}

    }
}
