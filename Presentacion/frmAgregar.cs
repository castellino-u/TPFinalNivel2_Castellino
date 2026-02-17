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
using System.IO;
using System.Configuration;


namespace Presentacion
{
    public partial class frmAgregar : Form
    {
        Articulo art = null;
        OpenFileDialog archivo = null;


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
            //Validaciones
            if(txtCodigo.Text == "")
            {
                erpCodigo.SetError(txtCodigo, "Debe ingresar un Código");
                return;
            }
            if(txtNombre.Text == "")
            {
                erpNombre.SetError(txtNombre, "Debe ingresar un nombre");
                return;
            }
            if(cboMarca.SelectedIndex == -1)
            {
                erpMarca.SetError(cboMarca, "Debe seleccionar una Marca");
                return;
            }
            if(cboCategoria.SelectedIndex == -1)
            {
                erpCategoria.SetError(cboCategoria, "Debe seleccionar una Categoría");
                return;
            }
            
            
            //Hay que capturar los datos de las estructuras de control en un objeto articulo y mandarlas a la base de datos
            //Captura de datos 
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();

            if (art == null)
            {
                art = new Articulo();
            }
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
            
            
            //Acá hay que ver qué método se ejecuta, si agregar() o modificar()
            if (art.Id > 0)
            {
                try
                {
                    articuloNegocio.Modificar(art);
                    MessageBox.Show("Modificado correctamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al modificar artículo: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    articuloNegocio.Agregar(art);
                    MessageBox.Show("Agregado correctamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar artículo: " + ex.Message);
                }
            }

            //Acá guardaremos una copia de la imagen local en donde querramos
            //...
            try
            {

                if((archivo != null) && File.Exists(archivo.FileName) && !(txtImagenUrl.Text.StartsWith("https", StringComparison.OrdinalIgnoreCase)))
                {
                    string carpeta = ConfigurationManager.AppSettings["images-articles"]; //guardamos la ruta y nombre de la carpeta
                    Directory.CreateDirectory(carpeta);                                   //si la carpeta no existe, la crea, si existe, no hace nada

                    string destino = Path.Combine(carpeta, archivo.SafeFileName);
                    File.Copy(archivo.FileName, destino, true);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al copiar la imagen: " + ex.Message);
            }

            this.Close();
        }

        private void frmAgregar_Load(object sender, EventArgs e)
        {
            cargarImagen("https://w7.pngwing.com/pngs/644/437/png-transparent-lost-and-found-property-gudi-padwa-miscellaneous-photography-logo-thumbnail.png");
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();

            //Desplegables
            //...
            cboCategoria.DataSource = categoriaNegocio.listar();
            cboCategoria.ValueMember = "Id";
            cboCategoria.DisplayMember = "Descripcion";
            cboCategoria.SelectedIndex = -1;

            cboMarca.DataSource = marcaNegocio.listar();
            cboMarca.ValueMember = "Id";
            cboMarca.DisplayMember = "Descripcion";
            cboMarca.SelectedIndex = -1;

            //hay que mostrar los datos si es que se presionó MODIFICAR, sino no hacer nada  
            //...


            
            if (!(art == null))
            {
                btnAgregar.Text = "Modificar";      //Detalle para que sea mas evidente que se quiere modificar un registro

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

        private void btnLevantarImg_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg|png|*.png";
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagenUrl.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
                //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-articles"] + archivo.SafeFileName);


            }
        }
    }
}
