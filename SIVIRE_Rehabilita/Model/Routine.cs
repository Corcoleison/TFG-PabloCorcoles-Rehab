using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIVIRE_Rehabilita.Model
{
    public class Routine
    {
        string name;
        List<Exercise> exercises;

        public string Name { get { return this.name; } }

        public List<Exercise> Exercises
        {
            get { return this.exercises; }
            set
            {
                this.exercises = new List<Exercise>();
                if (value != null && value.Count > 0)
                    this.exercises.AddRange(value);
            }
        }

        public Routine(string name, List<Exercise> exercises)
        {
            this.name = name;
            this.Exercises = exercises;
        }

        public List<Exercise> getUnfishiedExercise()
        {
            return Exercises.Where(x => x.Finished == false).ToList();
        }
    }
}
