using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using Xamarin.Forms;
using XF.Atividade3.Data;

namespace XF.Atividade3.Model
{
    public class Student
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string RM { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Approved { get; set; }
        public string IsApproved
        {
            get{return (Approved) ? "Aprovado" : "Reprovado";}
        }
    }

    public class StudentRepository {

        private StudentRepository() { }

        private static SQLiteConnection database;
        private static readonly StudentRepository instance = new StudentRepository();
        public static StudentRepository Instance {
            get {
                if (database == null) {
                    database = DependencyService.Get<ISQLite>().GetConexao();
                    database.CreateTable<Student>();
                }
                return instance;
            }
        }

        static object locker = new object();

        public static int SaveStudent(Student student) {
            lock (locker) {
                if (student.Id != 0) {
                    database.Update(student);
                    return student.Id;
                } else {
                    return database.Insert(student);
                }
            }
        }

        public static IEnumerable<Student> GetStudents() {
            lock (locker) {
                return (from c in database.Table<Student>() select c).ToList();
            }
        }

        public static Student GetStudent(int Id) {
            lock (locker) {
                return database.Table<Student>().Where(c => c.Id == Id).FirstOrDefault();
            }
        }

        public static int RemoveStudent(int Id) {
            lock (locker) {
                return database.Delete<Student>(Id);
            }
        }
    }

}