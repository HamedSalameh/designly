export function toastOptionsFactory() {
    // Get the direction of the document
    const isRtl = document.documentElement.dir === 'rtl';
  
    return {
      closeButton: true,
      progressBar: true,
      // Adjust positionClass based on text direction
      positionClass: isRtl ? 'toast-top-left' : 'toast-top-right',
      // Add other options as needed
      timeOut: 5000,
      // extendedTimeOut: 1000,
      // enableHtml: true,
      // preventDuplicates: true,
      resetTimeoutOnDuplicate: true,
      // toastClass: 'ngx-toastr',
    };
  }
  