export async function inicializarQuill(id, dotnetHelper, callbackMethod) {
    const quill = new Quill(`#${id}`, {
        theme: 'snow'
    });
    quill.on('text-change', () => {
        const html = quill.root.innerHTML;
        dotnetHelper.invokeMethodAsync(callbackMethod, html);
    });
    return quill;
}

export function definirContenidoQuill(instanciaQuill, contenido) {
    instanciaQuill.root.innerHTML = contenido || "";
}

