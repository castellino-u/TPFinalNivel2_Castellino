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
        Articulo art;

        public frmAgregar()
        {
            InitializeComponent();
            Text = "Agregar";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //Hay que capturar los datos de las estructuras de control en un objeto articulo y mandarlas a la base de datos
            //Captura de datos 
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            art = new Articulo();

            try
            {
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

                articuloNegocio.Agregar(art);
                MessageBox.Show("Agregado correctamente");


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void frmAgregar_Load(object sender, EventArgs e)
        {
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();

            cboCategoria.DataSource = categoriaNegocio.listar();
            cboMarca.DataSource = marcaNegocio.listar();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
