using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab11
{
    class Program
    {
        static void Main(string[] args)
        {

            var db = new DataClassesDataContext();
            int id = 100;

            if(db.Students.Any(s => s.Id == id))
            {
                Console.WriteLine($"Student with id {id} already exists");
            }
            else
            {
                var student = new Student() 
                    { DatumNarozeni = new DateTime(1992, 6, 1), Id = id, Jmeno = "Jirka", Prijmeni = "Novotny" };
                db.Students.InsertOnSubmit(student);
                db.SubmitChanges();
                Console.WriteLine($"Student with id {id} has been created");

            }

            

            IEnumerable<Student> stud = StudentiPredmetu("BOOP", db);
            IEnumerable<Predmet> pred = PredmetyStudentu(1000, db);


            var hod = db.Hodnocenis.GroupBy(x => x.ZkratkaPredmet);
            foreach (var predmet in hod)
            {
                var foo = hod.Select(x => x.Where(y => y.ZkratkaPredmet == predmet.Key).Average(z => z.Znamka));
                Console.WriteLine($" {predmet.Key} has average rating of");
                //DOESN'T WORK :(

            }


            Console.ReadLine();

        }

        private static IEnumerable<Predmet> PredmetyStudentu(int v1, DataClassesDataContext db)
        { 
            var studs = db.StudentPredmets.Where(x => x.IdStudent == v1).Select(x => x.ZkratkaPredmet).ToList();
            return db.Predmets.Where(x => studs.Contains(x.Zkratka));
        }

        private static IEnumerable<Student> StudentiPredmetu(string v1, DataClassesDataContext db)
        {
            var classes = db.StudentPredmets.Where(x => x.ZkratkaPredmet == v1).Select(x => x.IdStudent).ToList();
            return db.Students.Where(x => classes.Contains(x.Id));
        }
    }
}
