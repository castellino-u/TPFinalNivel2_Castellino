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

            //Cargamos los desplegables
            cboCampo.Items.Add("Precio");
            cboCampo.Items.Add("Marcas");
            cboCampo.Items.Add("Categorías");

            
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
            if(dgvArticulo.CurrentRow == null) { return;  }
            if(dgvArticulo.CurrentRow.DataBoundItem == null) { return;}
            
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
            cboCampo.SelectedIndex = -1;
            cboCriterio.SelectedIndex = -1;
            txtFiltroAvanzado.Text = "";
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

            listaFiltrada = listaArticulos.FindAll(x => x.Nombre.ToLower().Contains(filtro.ToLower()));
            if (listaFiltrada.Count <= 1)
            {
                listaFiltrada = listaArticulos.FindAll(x => x.Codigo.ToLower().Contains(filtro.ToLower()));
            }

            dgvArticulo.DataSource = null;
            dgvArticulo.DataSource = listaFiltrada;
            ocultarColumnas();


        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //Acá ejecutaremos el filtrado contra base de datos
            if(cboCampo.SelectedItem == null)
            {
                MessageBox.Show("funciona correcto la validaciopn");
                return;
            }
            if(cboCriterio.SelectedItem == null)
            {
                return;
            }

            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            string campo = cboCampo.SelectedItem.ToString();
            string criterio = cboCriterio.SelectedItem.ToString();

            if (campo == "Precio")
            {
                if (decimal.TryParse(txtFiltroAvanzado.Text, out decimal precio))
                {
                    //Acá debemos ejecutar la busqueda por filtro
                    try
                    {
                        dgvArticulo.DataSource = null;
                        dgvArticulo.DataSource = articuloNegocio.filtroAvanzado(campo, criterio, precio);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocurrió un error al buscar: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Debe ingresar solo números en el campo VALOR");
                    return;
                }
            }
            else
            {
                try
                {
                    dgvArticulo.DataSource = null;
                    dgvArticulo.DataSource = articuloNegocio.filtroAvanzado(campo, criterio);

                }
                catch (Exception ex)
                {

                    MessageBox.Show("Ocurrió un error al buscar: " + ex.Message);
                }
            }
        }


        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboCriterio.DataSource = null;
            cboCriterio.Items.Clear();
            if(cboCampo.SelectedItem == null)
            {
                return;
            }

            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
                txtFiltroAvanzado.Enabled = true; // deshabilita


            }
            else if (cboCampo.SelectedItem.ToString() == "Marcas")
            {
                txtFiltroAvanzado.Text = "";
                txtFiltroAvanzado.Enabled = false; // deshabilita

                MarcaNegocio marcaNegocio = new MarcaNegocio();
                cboCriterio.DataSource = marcaNegocio.listar();
                cboCriterio.SelectedIndex = -1;
            }
            else if(cboCampo.SelectedItem.ToString() == "Categorías")
            {
                txtFiltroAvanzado.Text = "";
                txtFiltroAvanzado.Enabled = false; // deshabilita

                CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
                cboCriterio.DataSource = categoriaNegocio.listar();
                cboCriterio.SelectedIndex = -1;

            }
        }

        
    }
}
