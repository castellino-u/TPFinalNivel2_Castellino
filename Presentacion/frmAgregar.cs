using Negocio;
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



namespace Presentacion
{
    public partial class frmAgregar : Form
    {
        Articulo art = null;

        public frmAgregar()
        {
            InitializeComponent();
            Text = "Agregar";

        }

        public frmAgregar(Articulo art)
        {
            this.art = art;
            InitializeComponent();
            Text = "Modificar";


        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //Hay que capturar los datos de las estructuras de control en un objeto articulo y mandarlas a la base de datos
            //Captura de datos 
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();

            try
            {
                if (!(art == null))
                {
                    art = new Articulo();
                    art.Codigo = txtCodigo.Text;
                    art.Nombre = txtNombre.Text;
                    art.Descripcion = txtDescripcion.Text;
                    art.ImagenUrl = txtImagenUrl.Text;

                    decimal precio;

                    if (decimal.TryParse(txtPrecio.Text, out precio))
                    {
                        art.Precio = precio;
                    }

                    art.Categoria = (Categoria)cboCategoria.SelectedItem;
                    art.Marca = (Marca)cboMarca.SelectedItem;

                }
                else
                {
                    //Si no es nulo, quiere decir que viene con un objeto y se requiere modificar ese objeto 

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            articuloNegocio.Agregar(art);
            MessageBox.Show("Agregado correctamente");

            this.Close();
        }

        private void frmAgregar_Load(object sender, EventArgs e)
        {
            cargarImagen("https://w7.pngwing.com/pngs/644/437/png-transparent-lost-and-found-property-gudi-padwa-miscellaneous-photography-logo-thumbnail.png");
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();

            cboCategoria.DataSource = categoriaNegocio.listar();
            cboCategoria.ValueMember = "Id";
            cboCategoria.DisplayMember = "Descripcion";
            cboCategoria.SelectedIndex = -1;

            cboMarca.DataSource = marcaNegocio.listar();
            cboMarca.ValueMember = "Id";
            cboMarca.DisplayMember = "Descripcion";
            cboMarca.SelectedIndex = -1;

            //Hay que capturar los datos de las estructuras de control en un objeto articulo y mandarlas a la base de datos o mostrar los datos del objeto, depende del método
            //...


            try
            {
                if (!(art == null))
                {
                   
                    txtCodigo.Text = art.Codigo;
                    txtNombre.Text = art.Nombre ;
                    txtDescripcion.Text = art.Descripcion;
                    txtImagenUrl.Text = art.ImagenUrl;
                    txtPrecio.Text = art.Precio.ToString();
                    cargarImagen(art.ImagenUrl);

                    //Queda mapear Categoría y Marca
                    cboCategoria.SelectedValue = art.Categoria.Id;
                    cboMarca.SelectedValue = art.Marca.Id;
                }
                else
                {
                    //Si no es nulo, quiere decir que viene con un objeto y se requiere modificar ese objeto 

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
        }

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
    }
}
