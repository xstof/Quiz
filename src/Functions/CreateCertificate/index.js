var Jimp = require("jimp");
var path = require("path");
var azure = require('azure-storage');

module.exports = function (context, data) {

    var AttemptId = data.AttemptId;
    var ScoreInPercentage = data.ScoreInPercentage;

    // Log incoming request:
    context.log('Got a request to create certificate for attempt id: ' + AttemptId + ' with score: ' + ScoreInPercentage);

    // Determine where base image is:
    var baseImgPath = path.resolve(__dirname, 'basecert.jpg');
    context.log('base image path: ' + baseImgPath);

    // Read image with Jimp
    Jimp.read(baseImgPath).then((image) => {
        context.log('got base certificate to start from');

        Jimp.loadFont(Jimp.FONT_SANS_32_BLACK).then(function (font) {
            context.log('Loaded font for writing on image.');

            // Write score on image:
            image.print(font, 50, 50, "Score: " + ScoreInPercentage + "%");

            Jimp.loadFont(Jimp.FONT_SANS_8_BLACK).then(function (font) {
                // Write attemptid on image:
                image.print(font, 50, 150, "Attempt: " + AttemptId);

                // Manipulate image
                image.getBuffer(Jimp.MIME_JPEG, (error, stream) => {
                    context.log(`Successfully processed the image`);

                    var blobLocation = generateSasToken(context, 'certificates', AttemptId + '.jpg', 'r').uri;
                    context.log('Certficate to be stored here: ' + blobLocation);

                    // Return url to storage:
                    context.res = {
                        body: { certificateUrl: blobLocation }
                    };

                    // Bind the stream to the output binding to create a new blob
                    context.done(null, { outputBlob: stream });
                });
            });

            // Check for errors
            if (error) {
                context.log(`There was an error processing the image.`);
                context.done(error);
            }
        });
    });

    function generateSasToken(context, container, blobName, permissions) {
        var connString = process.env.AzureWebJobsStorage;
        var blobService = azure.createBlobService(connString);

        // Create a SAS token that expires in an hour
        // Set start time to five minutes ago to avoid clock skew.
        var startDate = new Date();
        startDate.setMinutes(startDate.getMinutes() - 5);
        var expiryDate = new Date(startDate);
        expiryDate.setMinutes(startDate.getMinutes() + 60);

        permissions = permissions || azure.BlobUtilities.SharedAccessPermissions.READ;

        var sharedAccessPolicy = {
            AccessPolicy: {
                Permissions: permissions,
                Start: startDate,
                Expiry: expiryDate
            }
        };

        var sasToken = blobService.generateSharedAccessSignature(container, blobName, sharedAccessPolicy);

        return {
            token: sasToken,
            uri: blobService.getUrl(container, blobName, sasToken, true)
        };
    }

};