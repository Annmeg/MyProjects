using System.ComponentModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ArmTictactoe_2
{
    public class Country
    {
        public string Name { get; set; }
    }

    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="propName">Name of the property.</param>
        public void RaisePropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        ObservableCollection<Country> countryNames;

        public ObservableCollection<Country> CountryNames
        {
            get => this.countryNames;
            set
            {
                this.countryNames = value;
                RaisePropertyChanged("CountryNames");
            }
        }

        private GridItemsLayout countryCollectionViewItemsLayout;

        public GridItemsLayout CountryCollectionViewItemsLayout
        {
            get => countryCollectionViewItemsLayout;

            set
            {
                this.countryCollectionViewItemsLayout = value;
                RaisePropertyChanged("CountryCollectionViewItemsLayout");
            }
        }

        internal void SizeChanged(double width, double height)
        {
            if (width > height)
            {
                CountryCollectionViewItemsLayout = new GridItemsLayout(4, ItemsLayoutOrientation.Vertical);
            }
            else
            {
                CountryCollectionViewItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical);
            }
        }

        public MainViewModel()
        {
            CountryCollectionViewItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical);
            CountryNames = new ObservableCollection<Country>() {
            new Country() { Name = "Country1" }, new Country() { Name = "Country2" }, new Country() { Name = "Country3" },
            new Country() { Name = "Country4" }, new Country() { Name = "Country5" }, new Country() { Name = "Country6" },
            new Country() { Name = "Country7" }, new Country() { Name = "Country8" }, new Country() { Name = "Country9" },
            new Country() { Name = "Country10" }
        };

        }
    }
}
