﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
// Agregar los namespaces de conexión con SQL Server
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

namespace Clinica_Dental
{
    /// <summary>
    /// Lógica de interacción para HistorialClinico.xaml
    /// </summary>
    public partial class historialClinico : Window
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["Clinica_Dental.Properties.Settings.ClinicaDentalConnectionString"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);


        // Variables miembro
        private HistorialesClinicos consulta = new HistorialesClinicos();
        private List<HistorialesClinicos> historialClinicos;

        public historialClinico(string identidadPaciente)
        {
            InitializeComponent();
            // Llenar el combobox de estado
            cmbEstado.ItemsSource = Enum.GetValues(typeof(estadoClinico));
            txtIdentidadPaciente.Text = identidadPaciente;
            // Llenar el listbox de historialClinico
            ObtenerHistorialClinico();


        }
        
        private void LimpiarFormulario()
        {
            txtObservaciones.Text = string.Empty;
            txtAfecciones.Text = string.Empty;
            txtHistorialClinico.Text = string.Empty;
            cmbEstado.Text = null;
        }
        private void ObtenerHistorialClinico()
        {
            historialClinicos = consulta.MostrarHistorialClinico(txtIdentidadPaciente.Text);
            dgvHistorialClinico.ItemsSource = historialClinicos;

        }
        private void ObtenerValoresFormulario()
        {
            
            consulta.FechaCreacion = DateTime.Now;
            consulta.Afecciones = txtAfecciones.Text;
            consulta.Observaciones = txtObservaciones.Text;
            consulta.identidadPaciente = txtIdentidadPaciente.Text;
            consulta.Estado = (estadoClinico)cmbEstado.SelectedItem;
        }
        private bool VerificarValores()
        {
            if (txtAfecciones.Text == string.Empty ||
                txtObservaciones.Text == string.Empty )
            {
                MessageBox.Show("Por favor ingresa todos los valores en las cajas de texto");
                return false;
            }
            else if (cmbEstado.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona el estado del HistorialClinico");
                return false;
            }
            

            return true;
        }
        private void Inhabilitar()
        {
            btnAgregar.IsEnabled = false;
            txtHistorialClinico.IsEnabled = false;
        }
        private void Habilitar()
        {
            btnAgregar.IsEnabled = true;
        }
        private void ValoresFormularioDesdeObjeto()
        {
            txtObservaciones.Text = consulta.Observaciones;
            txtAfecciones.Text = consulta.Afecciones;
            txtHistorialClinico.Text = Convert.ToInt32(consulta.IdHistorialClinico).ToString();
            txtIdentidadPaciente.Text = consulta.identidadPaciente;
            cmbEstado.SelectedValue = consulta.Estado;

        }


        private void txtAfecciones_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvHistorialClinico.SelectedValue == null)
                    MessageBox.Show("Por favor selecciona un historial desde el listado");
                else
                {
                    // Mostrar un mensaje de confirmación
                    MessageBoxResult result = MessageBox.Show("¿Deseas eliminar un historial?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Eliminar la habitación
                        consulta.EliminarHistorialConsulta(consulta);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al momento de eliminar el historial...");
                Console.WriteLine(ex.Message);
                
            }
            finally
            {
                // Actualizar los pacientes
                ObtenerHistorialClinico();

                LimpiarFormulario();
                Habilitar();
            }

        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            // Verificar que se ingresaron los valores requeridos
            if (VerificarValores())
            {
                try
                {
                    consulta = consulta.BuscarHistorialClinico(txtIdentidadPaciente.Text);

                    if (consulta.identidadPaciente == null)
                    {
                        // Obtener los valores para el historial
                        ObtenerValoresFormulario();

                        // Insertar los datos del historial
                        consulta.CrearHistorialClinico(consulta);

                        // Mensaje de inserción exitosa
                        MessageBox.Show("¡Datos insertados correctamente!");
                        LimpiarFormulario();

                    }
                    else
                    {
                        MessageBox.Show("Ya cuenta con un historial clínico");
                        ValoresFormularioDesdeObjeto();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha ocurrido un error al momento de insertar el historialClinico...");
                    Console.WriteLine(ex.Message);
                    
                }
                finally
                {

                    ObtenerHistorialClinico();
                }

            }

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (dgvHistorialClinico.SelectedValue == null)
                MessageBox.Show("Por favor selecciona un historial desde el listado");
            else
            {
                if (VerificarValores())
                {
                    try
                    {
                        // Obtener los valores para el historialClinico desde el formulario
                        ObtenerValoresFormulario();

                        // Mostrar un mensaje de confirmación
                        MessageBoxResult result = MessageBox.Show("¿Deseas modificar la historial?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (result == MessageBoxResult.Yes)
                        {
                            // Modificar historialClinico
                            consulta.ModificarHistorialClinico(consulta);

                            // Mensaje de actualización realizada
                            MessageBox.Show("¡Historial modificada correctamente!");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ha ocurrido un error al momento de modificar el historial ...");
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        // Actualizar los historialClinico
                        ObtenerHistorialClinico();

                        LimpiarFormulario();
                        Habilitar();
                    }
                }
            }

        }

        private void btnCita_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Implementar la búsqueda del historialClinico desde la clase HistorialClinico
                HistorialesClinicos elHistorial = consulta.BuscarHistorialClinico(txtIdentidadPaciente.Text);

                // Verificar si el historialClinico existe
                if (elHistorial.IdHistorialClinico == null)
                    MessageBox.Show("El historial clínico no ha sido seleccionado. Favor verificar.");
                else
                {
                    cita lacita = new cita(Convert.ToInt32(elHistorial.IdHistorialClinico));

                    lacita.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        private void dgvHistorialClinico_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            HistorialesClinicos HistorialSelecionado = dgvHistorialClinico.SelectedItem as HistorialesClinicos;
            consulta = consulta.BuscarHistorialClinico(HistorialSelecionado.identidadPaciente);

            ValoresFormularioDesdeObjeto();
            Inhabilitar();

        }

        private void dgvHistorialClinico_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnConsulta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Implementar la búsqueda del historiallinico desde la clase HistorialClinico
                HistorialesClinicos loshistorialesClinicos = consulta.BuscarHistorialClinico2(Convert.ToInt32(txtHistorialClinico.Text));

                // Verificar si el historialClinico existe
                if (Convert.ToInt32(loshistorialesClinicos.IdHistorialClinico).ToString() == null)
                    MessageBox.Show("El historialClinico no ha sido seleccionado. Favor verificar.");
                else
                {
                    historialConsulta historialConsultas = new historialConsulta(Convert.ToInt32(loshistorialesClinicos.IdHistorialClinico));

                    historialConsultas.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
            }

        }
    }
}
