using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;


namespace Presentacion
{
    public partial class Form1 : Form
    {

        List<Articulo> listaArticulos;

        public Form1()
        {
            InitializeComponent();
            Text = "Catálogo";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargarDatos();
        }
         //Metodo para cargar la descripción de los objetos en los labels
         //...
        private void cargarDescripcion(Articulo aux)
        {
            lblNombre.Text = aux.Nombre;
            lblPrecio.Text = aux.Precio.ToString();
            lblDescripcion.Text = aux.Descripcion;
            lblMarca.Text = aux.Marca.Descripcion;
            lblCategoria.Text = aux.Categoria.Descripcion;
        }



        //Método para formatear las columnas del dataGridView
        //...
        private void ocultarColumnas()
        {
            dgvArticulo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvArticulo.Columns["Id"].Visible = false;
            dgvArticulo.Columns["ImagenUrl"].Visible = false;
            dgvArticulo.Columns["Descripcion"].Visible = false;
        }

        private void pbxArticulo_Click(object sender, EventArgs e)
        {

        }

        //Método para cargar imagen en pictureBox
        //...
        public void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception)
            {
                pbxArticulo.Load("https://w7.pngwing.com/pngs/644/437/png-transparent-lost-and-found-property-gudi-padwa-miscellaneous-photography-logo-thumbnail.png");
            }
        }

        private void dgvArticulo_SelectionChanged(object sender, EventArgs e)
        {
            Articulo articulo = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
            cargarImagen(articulo.ImagenUrl);
            cargarDescripcion(articulo);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregar agregar = new frmAgregar();
            agregar.ShowDialog();
            cargarDatos();
        }
        public void cargarDatos()
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            
            listaArticulos = articuloNegocio.listar();
            dgvArticulo.DataSource = listaArticulos;
            cargarImagen(listaArticulos[0].ImagenUrl);
            ocultarColumnas();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //Acá reutilizamos el form de agregar
            Articulo seleccionado =  (Articulo)dgvArticulo.CurrentRow.DataBoundItem;

            frmAgregar modificar = new frmAgregar(seleccionado);
            modificar.ShowDialog();
            cargarDatos();


        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            cargarDatos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            try
            {
                Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                ArticuloNegocio articuloNegocio = new ArticuloNegocio();

                DialogResult resultado = MessageBox.Show("¿Deseas realmente eliminar el registro?", "Eliminado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.Yes)
                {
                    articuloNegocio.eliminar(seleccionado.Id);
                    MessageBox.Show("Realizado", "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch 
            {

                MessageBox.Show("Error al eliminar");
            }

            cargarDatos();
        }

        private void txtFiltroRapido_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtFiltroRapido.Text;
            List<Articulo> listaFiltrada;

            if (int.TryParse(txtFiltroRapido.Text, out int num))
            {

                listaFiltrada = listaArticulos.FindAll(x => x.Id == num);

            }
            else
            {
                listaFiltrada = listaArticulos.FindAll(x => x.Nombre.ToLower().Contains(filtro.ToLower()));
            }
            dgvArticulo.DataSource = null;
            dgvArticulo.DataSource = listaFiltrada;

        }
    }
}
