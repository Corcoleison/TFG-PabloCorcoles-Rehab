using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SIVIRE_Rehabilita.Model
{
    public class Patient
    {
        #region Attributes

        string name;
        DateTime birthDate;
        string sex;
        ImageSource photo = null;
        List<Routine> routines;

        string nickName;
        string password;

        #endregion

        #region Properties

        public string Name { get { return this.name; } }
        public DateTime BirthDate { get { return this.birthDate; } }
        public string Sex { get { return this.sex; } }
        public ImageSource Photo { get { return this.photo; } }
        public List<Routine> Routines 
        { 
            get { return this.routines; } 
            set
            {
                this.routines = new List<Routine>(); 
                if (value != null && value.Count > 0)
                    this.routines.AddRange(value);
            }
        }

        public string NickName
        {
            get { return this.nickName; }
            set { this.nickName = value; }
        }

        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        #endregion

        public Patient(string name, DateTime birthDate, string sex)
        {
            this.name = name;
            this.birthDate = birthDate;
            this.sex = sex;

            Uri imagePath = new Uri("Images/user_default_"+this.sex+".png", UriKind.Relative);
            if (imagePath != null)
                this.photo = new BitmapImage(new Uri(((App)Application.Current).BaseUri, imagePath));
        }
    }
}
