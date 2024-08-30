

namespace MauiSqliteDemo3381809
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbService _dbService;
        private int _editClienteId;

        public MainPage(LocalDbService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            Task.Run(async()=> ListView.ItemsSource = await _dbService.GetCliente());
        }

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            if (_editClienteId==0)
            {
                await _dbService.Create(new Cliente
                {
                    NombreCliente = nombreEntryField.Text,
                    Email = emailEntryField.Text,
                    Movil = movilEntryField.Text
                });

              else
                {
                    await _dbService.Update(new Cliente
                    {
                       Id=_editClienteId,
                       NombreCliente=nombreEntryField.Text,
                       Email=emailEntryField.Text,
                       Movil=movilEntryField.Text
                    });

                    _editClienteId = 0;
                }
                nombreEntryField.Text = string.Empty;
                emailEntryField.Text = string.Empty;
                movilEntryField.Text = string.Empty;

                ListView.ItemsSource = await _dbService.GetCliente();
            }
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var cliente = (Cliente)e.Item;
            var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

            switch(action)
            {
                case "Edit":
                    _editClienteId = cliente.Id;
                    nombreEntryField.Text = cliente.NombreCliente;
                    emailEntryField.Text= cliente.Email;
                    movilEntryField.Text=cliente.Movil;
                    break;

                case "Delete":
                    await _dbService.Delete(cliente);
                    ListView.ItemsSource = await _dbService.GetCliente();
                    break;

            }
        }
    }
}
