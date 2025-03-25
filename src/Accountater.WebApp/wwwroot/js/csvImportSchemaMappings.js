document.addEventListener('DOMContentLoaded', () => {
    const container = document.getElementById('mappings-container');
    const addBtn = document.getElementById('add-mapping');
    const form = document.getElementById('csv-import-form');
    const template = document.getElementById('mapping-template')?.innerHTML;
    let mappingIndex = container.children.length;

    addBtn?.addEventListener('click', () => {
        const html = template.replaceAll('__index__', mappingIndex);
        container.insertAdjacentHTML('beforeend', html);
        mappingIndex++;
    });

    container?.addEventListener('click', (e) => {
        if (e.target.classList.contains('remove-mapping')) {
            e.target.closest('.mapping-entry').remove();
        }
    });

    form?.addEventListener('submit', () => {
        renumberMappings();
    });

    function renumberMappings() {
        const entries = document.querySelectorAll('#mappings-container .mapping-entry');
        entries.forEach((entry, index) => {
            entry.dataset.index = index;

            entry.querySelectorAll('input').forEach(input => {
                const name = input.getAttribute('name');
                if (!name) return;

                const newName = name.replace(/\[\d+\]/, `[${index}]`);
                input.setAttribute('name', newName);
            });
        });

        // Update the global index so future "Add" clicks don't reuse an old index
        mappingIndex = container.children.length;
    }
});
