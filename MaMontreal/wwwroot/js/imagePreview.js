window.addEventListener('DOMContentLoaded',
    function () {
        $("#ImageUrl").change(function () {
            readURL(this);
        });
    });

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#image_upload_preview').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

//TODO: Some changes will be needed once this webapp gets hosted
// https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blob-dotnet-get-started?tabs=azure-ad