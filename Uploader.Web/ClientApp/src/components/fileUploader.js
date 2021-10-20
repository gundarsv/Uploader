import React from 'react';
import Loader from 'react-loader-spinner';
import * as apiService from '../services/apiService';

const FileUploader = () => {
    const [result, setResult] = React.useState('');
    const [isLoading, setIsLoading] = React.useState(false);
    const [file, setFile] = React.useState();
    const [fileName, setFileName] = React.useState('');
    const [comments, setComments] = React.useState('');

    async function uploadFile(file, fileName, comments) {
        setIsLoading(true);
        await apiService
            .addFile(file, fileName, comments)
            .then(() => {
                setResult('File was uploaded');
            })
            .catch((error) => {
                if (error.response) {
                    setResult(error.response.data);
                } else if (error.request) {
                    setResult(error.request);
                } else {
                    setResult(error.message);
                }
            });

        setIsLoading(false);
    }

    return isLoading ? (
        <div style={{ textAlign: 'center' }}>
            <Loader type="TailSpin" color="#9b4dca" height={50} width={50} />
        </div>
    ) : (
        <div>
            <p style={{ color: 'darkred' }}>{result}</p>
            <fieldset>
                <label htmlFor="fileNameField">File name</label>
                <input type="text" value={fileName} onChange={(e) => setFileName(e.target.value)} id="fileNameField" />
                <label htmlFor="commentField">Comment</label>
                <textarea value={comments} id="commentField" onChange={(e) => setComments(e.target.value)} />
                <label htmlFor="fileField">File</label>
                <input type="file" id="fileField" onChange={(e) => setFile(e.target.files[0])} />
                <input className="button-primary float-right" type="submit" onClick={() => uploadFile(file, fileName, comments)} value="Send" />
            </fieldset>
        </div>
    );
};

export default FileUploader;
