//import Quill from 'quill';

// Crea un editor de Quill y devuelve su referencia
export async function createQuillEditor(_editorId, _placeholder, _minHeight, _dotnetRef, _callbackMethod) {
	// Obtener el <div> con la GUID
	const container = document.getElementById(_editorId);
	// Crear objeto de Quill
	const quill = new Quill(container, {
		theme: 'snow',
		placeholder: _placeholder
	});
	// Crear un 'Listener' para cualquier cambio de texto
	quill.on('text-change', () => {
		const html = container.querySelector('.ql-editor').innerHTML;
		_dotnetRef.invokeMethodAsync(_callbackMethod, html);
	});
	return quill;
}

export function setQuillEditorContent(_quillInstance, _htmlContent) {
	_quillInstance.root.innerHTML = _htmlContent;
}
