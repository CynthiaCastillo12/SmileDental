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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Clinica_Dental
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Objeto de tipo usuario para implementar su funcionalidad
        private Usuario usuario = new Usuario();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonIniciar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Implementar la búsqueda del usuario desde la clase Usuario
                Usuario elUsuario = usuario.BuscarUsuario(txtUsuario.Text);

                // Verificar si el usuario existe
                if (elUsuario.Username == null)
                    MessageBox.Show("El usuario o la contraseña no es correcta. Favor verificar.");
                else
                {
                    // Verificar que la contraseña ingresada es igual a la contraseña
                    // almacenada en la base de datos
                    if (elUsuario.Password == pwbContraseña.Password && elUsuario.Estado)
                    {
                        txtUsuario.Text = string.Empty;
                        pwbContraseña.Clear();
                        MessageBox.Show("Bienvenido al sistema!!!.");
                        if (elUsuario.TipoUsuario == "Administrador")
                        {
                            menuPrincipal menu = new menuPrincipal();

                            menu.Show();
                        }
                        else
                        {
                            menuPrincipal2 menu2 = new menuPrincipal2();

                            menu2.Show();
                        }
                    }
                    else if (!elUsuario.Estado)
                        MessageBox.Show("Tu usuario se encuentra innactivo. Favor comunicarte con el personal de IT");
                    else
                        MessageBox.Show("El usuario o la contraseña no es correcta. Favor verificar.");
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
