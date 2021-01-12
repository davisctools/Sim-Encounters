mergeInto(LibraryManager.library, {
  SetCanvasCursor: function (str) {
    if (typeof this.canvas === 'undefined') {
      this.canvas = document.getElementById("#canvas");
    }
    this.canvas.style.cursor = Pointer_stringify(str);
  }
});