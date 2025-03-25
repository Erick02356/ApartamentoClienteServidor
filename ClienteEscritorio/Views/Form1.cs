using ClienteEscritorio.Models;
using ClienteEscritorio.Service;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows.Forms;

namespace ClienteEscritorio
{
    public partial class Form1 : Form
    {
        private readonly ApiService _apiService;
        private int? _editandoId = null;
        private HubConnection _hubConnection;

        public Form1()
        {
            InitializeComponent();
            _apiService = new ApiService();
            IniciarSignalR();

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await CargarApartamentos();

        }

        private async void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (DgvApto.SelectedRows.Count == 0) return;

            var id = (int)DgvApto.SelectedRows[0].Cells["ApartamentoId"].Value;
            var resultado = await _apiService.DeleteApartamentoAsync(id);

            if (resultado)
            {
                MessageBox.Show("Apartamento eliminado.");
                await CargarApartamentos();
            }
            else
            {
                MessageBox.Show("Error al eliminar.");
            }
        }


        private async void BtnCrear_Click(object sender, EventArgs e)
        {

            var apartamento = new ApartamentoViewModel
            {
                ApartamentoId = _editandoId ?? 0,
                Numero = txtNumero.Text,
                UsuarioResponsable = txtUsuarioResponsable.Text,
                Estado = CbEstado.Text,
                Torre = txtTorre.Text,
                Descripcion = txtDescripcion.Text,
                Piso = (int)numPiso.Value,
                AreaM2 = (double)numArea.Value,
            };
            bool resultado;
            if (_editandoId == null) // Si no hay ID, agregar nuevo
            {
                resultado = await _apiService.CreateApartamentoAsync(apartamento);
                if (resultado) MessageBox.Show("Apartamento agregado correctamente.");
            }
            else // Si hay ID, actualizar
            {
                resultado = await _apiService.UpdateApartamentoAsync(_editandoId.Value, apartamento);
                if (resultado) MessageBox.Show("Apartamento actualizado correctamente.");
                BtnCrear.Text = "Crear";

            }

            if (resultado)
            {
                await CargarApartamentos();
                LimpiarFormulario();
            }
            else
            {
                MessageBox.Show("Error al guardar los datos.");
            }
        }


        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            BtnCrear.Text = "Crear";
            LimpiarFormulario();
        }

        private async void DgvApto_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnCrear.Text = "Editar";
                var id = (int)DgvApto.Rows[e.RowIndex].Cells["ApartamentoId"].Value;
                var apartamento = await _apiService.GetApartamentoByIdAsync(id);
                if (apartamento != null)
                {
                    _editandoId = apartamento.ApartamentoId;
                    txtNumero.Text = apartamento.Numero;
                    txtUsuarioResponsable.Text = apartamento.UsuarioResponsable;
                    CbEstado.Text = apartamento.Estado;
                    txtTorre.Text = apartamento.Torre;
                    txtDescripcion.Text = apartamento.Descripcion;
                    numPiso.Value = apartamento.Piso;
                    numArea.Value = (decimal)apartamento.AreaM2;
                }
            }

        }





        #region
        private void LimpiarFormulario()
        {
            _editandoId = null;
            txtNumero.Clear();
            txtUsuarioResponsable.Clear();
            CbEstado.SelectedValue = -1;
            txtTorre.Clear();
            txtDescripcion.Clear();
            numPiso.Value = 1;
            numArea.Value = 10;
        }

        private async void IniciarSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7243/apartamentosHub") // URL de la API
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On("RecargarDatos", () =>
            {
                Invoke(new Action(() => CargarApartamentos())); // Recargar DataGridView
            });

            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("Conectado a SignalR");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con SignalR: {ex.Message}");
            }
        }
        private async Task CargarApartamentos()
        {
            DgvApto.DataSource = null;
            var apartamentos = await _apiService.GetApartamentosAsync();
            DgvApto.DataSource = apartamentos;
        }
        #endregion


    }
}
