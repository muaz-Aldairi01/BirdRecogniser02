//const serviceUrl = 'https://localhost:44380/api/ImageClassification/classifyImage';

//const serviceUrl = 'http://localhost:5000/api/ImageClassification/classifyImage';

const serviceUrl = 'api/RecogniseAPI/classifyImage';
const form = document.querySelector('form');

form.addEventListener('submit', e => {
    e.preventDefault();

    const files = document.querySelector('[type=file]').files;
    const formData = new FormData();

    formData.append('imageFile', files[0]);

    // If multiple files uploaded at the same time:
    //for (let i = 0; i < files.length; i++) {
    //    let file = files[i];
    //
    //    formData.append('imageFile[]', file);
    //}


    fetch(serviceUrl, {
        method: 'POST',
        body: formData
    })
        .then((resp) => resp.json())
        .then(function (response) {
            console.info('Response', response);
            console.log('Response', response);

            
            document.getElementById('divPrediction0').innerHTML = response[0].predictedLabel;
            document.getElementById('divProbability0').innerHTML = (response[0].probability * 100).toFixed(2) + "%";

            document.getElementById('divPrediction1').innerHTML = response[1].predictedLabel;
            document.getElementById('divProbability1').innerHTML = (response[1].probability * 100).toFixed(2) + "%";

            document.getElementById('divPrediction2').innerHTML = response[2].predictedLabel;
            document.getElementById('divProbability2').innerHTML = (response[2].probability * 100).toFixed(2) + "%";
            
            return response;
        });
            
            


});