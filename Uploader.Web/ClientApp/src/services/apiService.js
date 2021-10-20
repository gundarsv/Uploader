import axios from 'axios';

function getSettings() {
    return axios.get('api/settings');
}

function getFileExtensions() {
    return axios.get('api/settings/fileExtensions');
}

function removeSettings(id) {
    return axios.delete(`api/settings/${id}`);
}

function enableSettings(id) {
    return axios.put(`api/settings/${id}/enable`);
}

function addSettings(maxFileSize, maxHeight, maxWidth, minHeight, minWidth) {
    return axios.post(`api/settings`, { maxFileSize, maxHeight, maxWidth, minHeight, minWidth });
}

function removeFileExtensionFromSettings(settingsId, fileExtensionId) {
    return axios.delete(`api/settings/${settingsId}/FileExtensions/${fileExtensionId}`);
}

function addFileExtensionToSettings(settingsId, fileExtensionId) {
    return axios.post(`api/settings/${settingsId}/FileExtensions/${fileExtensionId}`);
}

function addFileExtension(fileExtension) {
    return axios.post(`api/settings/FileExtensions`, { fileExtension });
}

function addFile(file, fileName, comments) {
    var formData = new FormData();
    formData.append('File', file);
    formData.append('FileName', fileName);
    formData.append('Comment', comments);

    return axios.post(`api/file`, formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
    });
}

function removeFile(id) {
    return axios.delete(`api/file/${id}`);
}

function getFiles(pageNumber, pageSize) {
    return axios.get(`api/file/page/${pageNumber}/size/${pageSize}`);
}

function getFile(id) {
    return axios.get(`api/file/${id}`, { responseType: 'blob' });
}

export {
    getFiles,
    getSettings,
    getFile,
    addFile,
    removeFile,
    enableSettings,
    removeSettings,
    getFileExtensions,
    addSettings,
    removeFileExtensionFromSettings,
    addFileExtensionToSettings,
    addFileExtension,
};
