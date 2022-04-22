using ParkyAPI.Model.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static ParkyAPI.Model.Trail;

namespace ParkyAPI.Model
{
    public class TrailDtos
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }

        public double Elevation { get; set; }
        public DifficultyType Difficulty { get; set; }
        [Required]
        public int NationalParkId { get; set; }
        [ForeignKey("NationalParkId")]
        public NationalParkDtos NationalPark { get; set; }

      
    }
}
