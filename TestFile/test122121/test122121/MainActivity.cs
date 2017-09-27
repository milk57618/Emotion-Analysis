using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using System.Collections.Generic;
using Android.Content;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace test122121
{
    [Activity(Label = "test122121", MainLauncher = true)]
    public class MainActivity : Activity
    {
        static TextView txt;
        static ListView lstVehicles;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
          
            txt = FindViewById<TextView>(Resource.Id.titleText);
            lstVehicles = FindViewById<ListView>(Resource.Id.vehicleslist);
            txt.Text = "Testing Azure Functions from Xamarin!";

            ActiveAzureFunction();
        }

        public async void ActiveAzureFunction()
        {
            VehiclesAzureFunction vehiclesAzureFunction = new VehiclesAzureFunction("https://rcamerafunction.azurewebsites.net");
            var vehiclesList = await vehiclesAzureFunction.GetVehiclesHardCodedAsync();
            VehicleAdapter vadpt;
            vadpt = new VehicleAdapter(this, vehiclesList);
            lstVehicles.Adapter = vadpt;
        }

        public class VehiclesAzureFunction
        {
            private string v;

            public VehiclesAzureFunction() { }

            public VehiclesAzureFunction(string v)
            {
                this.v = v;
            }

            public async Task<List<Vehicle>> GetVehiclesHardCodedAsync()
            {
                var client = new System.Net.Http.HttpClient();
                string url = $"https://rcamerafunction.azurewebsites.net/api/HttpTriggerCSharp1?code=jIdXBMbxiysszZ4QdWPyZU3CLrvdwzu7vGQVardmVivzIV7qxsFJww==&make=Chevrolet";
                var response = await client.GetAsync(url);
                var vehiclesJson = response.Content.ReadAsStringAsync().Result;
                List<Vehicle> listOfVehicles = JsonConvert.DeserializeObject<List<Vehicle>>(vehiclesJson);
                return listOfVehicles;
            }
        }
    }
    
    public class Vehicle
    {
        public string Make { get; set; }
        public string Model { get; set; }

    }
    public class VehicleAdapter : BaseAdapter<Vehicle>
    {
        public List<Vehicle> vList;
        private Context vContext;
        public VehicleAdapter(Context context, List<Vehicle> list)
        {
            vList = list;
            vContext = context;
        }
        public override Vehicle this[int position]
        {
            get { return vList[position]; }
        }
        public override int Count
        {
            get { return vList.Count; }
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            try
            {
                if (row == null)
                {
                    row = LayoutInflater.From(vContext).Inflate(Resource.Layout.Main, null, false);
                }
                TextView txtModel = row.FindViewById<TextView>(Resource.Id.titleText);
                txtModel.Text = vList[position].Model;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally { }
            return row;
        }
    }
}

