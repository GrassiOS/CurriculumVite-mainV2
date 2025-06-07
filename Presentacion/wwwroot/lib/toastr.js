window.ShowToastr = (type, message) => {
   if (type === "success") {
     toastr.success(message, "Éxito: ", { timeOut: 5000 });
   }

   if (type === "error") {
      toastr.error(message, "Error: ", { timeOut: 5000 });
   }

   if (type === "warning") {
      toastr.warning(message, "Precaución: ", { timeOut: 5000 })
   }

   if (type === "info") {
      toastr.info(message, "Iformacipon: ", { timeOut: 5000 })
   }
}
