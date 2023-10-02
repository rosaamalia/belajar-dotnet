using api.Context;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace api.Models{
    // Model Employee
    [Index(nameof(Phone), nameof(Email), IsUnique = true)]
    public class Employee {
        // aturan membuat primary key -> namanya id, employeeID, employee_id -< gaperlu keyword [Key]
        [Key] // untuk membuat atribut sebagai primary key. jadi, atribut yg ditulis di bawahnya akan menjadi primary key
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public Account Account { get; set; }
    }
    public enum Gender {
        // tipe data class untuk menyediakan pilihan
        // digunakan sebagai tipe data untuk atribut
        // cara mengaksesnya dengan memanggil atribut enumerasinya
        Male,
        Female,
        Rather_Not_Say
    }
}