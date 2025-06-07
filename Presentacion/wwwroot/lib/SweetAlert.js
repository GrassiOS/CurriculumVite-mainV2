window.SweetAlertHelper = {
   showAlert: function (titulo, texto, icono) {
      Swal.fire({
         title: titulo,
         text: texto,
         icon: icono,
         confirmButtonText: 'OK'
      });
   },
   showConfirmation: function (titulo, texto) {
      return Swal.fire({
         title: titulo,
         text: texto,
         icon: 'warning',
         showCancelButton: true,
         confirmButtonText: 'Yes',
         cancelButtonText: 'NO'
      }).then((result) => {
         // Devuelve `true` si el usuario confirma, `false` si cancela
         return result.isConfirmed;
      });
   }
}


function MostrarModalConfirmacionBorrado() {
  $('#modalConfirmacionBorrado').modal('show');
}

function OcultarModalConfirmacionBorrado() {
  $('#modalConfirmacionBorrado').modal('hide');
}