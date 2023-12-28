//window.onload = function () {
//    document.getElementById("download").addEventListener("click", () => {
//        const cvPdf = this.document.getElementById("cvPdf");

//        var opt = {
//            margin: 1,
//            filename: 'myfile.pdf',
//            image: { type: 'jpeg', quality: 0.98 },
//            html2canvas: { scale: 2 },
//            jsPDF: { unit: 'in', format: 'letter', orientation: 'portrait' },
//            pagebreak: { mode: ['avoid-all', 'css', 'legacy'] }, // Sayfa kesme modu
//            jsPDF: { unit: 'in', format: 'letter', orientation: 'portrait' },
//            html2canvas: {
//                scale: 2,
//                windowWidth: window.innerWidth, // Genişlik
//                windowHeight: window.innerHeight, // Yükseklik
//            }
//        };

//        html2pdf().from(cvPdf).set(opt).save();
//    })
//}
document.addEventListener("DOMContentLoaded", () => {
    // Escuchamos el click del botón
    const $boton = document.querySelector("#download");
    $boton.addEventListener("click", () => {
        const $elementoParaConvertir = document.getElementById("drag"); // <-- Aquí puedes elegir cualquier elemento del DOM
        html2pdf()
            .set({
                margin: 1,
                filename: 'documento.pdf',
                image: {
                    type: 'jpeg',
                    quality: 0.98
                },
                html2canvas: {
                    scale: 3, // A mayor escala, mejores gráficos, pero más peso
                    letterRendering: true,
                },
                jsPDF: {
                    unit: "in",
                    format: "a3",
                    orientation: 'portrait' // landscape o portrait
                }
            })
            .from($elementoParaConvertir)
            .save()
            .catch(err => console.log(err));
    });
});