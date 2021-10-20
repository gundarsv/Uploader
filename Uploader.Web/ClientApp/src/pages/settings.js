import React from 'react';
import Loader from 'react-loader-spinner';
import { Link } from 'react-router-dom';
import Navigation from '../components/navigation';
import Table from '../components/table';
import * as apiService from '../services/apiService';
import { useCallback } from 'react';
import { AiFillMinusCircle } from 'react-icons/ai';
import Modal from 'react-modal';

Modal.setAppElement('#root');

function Settings() {
    const [settingsData, setSettingsData] = React.useState([]);
    const [fileExtensionsData, setFileExtensionsData] = React.useState([]);
    const [selectedFileExtension, setSelectedFileExtension] = React.useState(undefined);
    const [isLoading, setIsLoading] = React.useState(true);
    const [modalIsOpen, setIsOpen] = React.useState(false);
    const [settingsForModal, setSettingsForModal] = React.useState(0);
    const [result, setResult] = React.useState('');

    const customStyles = {
        content: {
            top: '50%',
            left: '50%',
            right: 'auto',
            bottom: 'auto',
            marginRight: '-50%',
            transform: 'translate(-50%, -50%)',
        },
    };

    React.useEffect(() => {
        setIsLoading(true);
        getSettings();
        getFileExtensions();
        setIsLoading(false);
    }, []);

    async function getSettings() {
        await apiService
            .getSettings()
            .then((response) => {
                setSettingsData(response.data);
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
    }

    const getFileExtensions = useCallback(async () => {
        await apiService
            .getFileExtensions()
            .then((response) => {
                setFileExtensionsData(response.data);
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
    }, []);

    const enableSettings = useCallback(async (id) => {
        setIsLoading(true);
        await apiService
            .enableSettings(id)
            .then(async () => {
                await getSettings();
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
    }, []);

    const removeSettings = useCallback(async (id) => {
        setIsLoading(true);
        await apiService
            .removeSettings(id)
            .then(async () => {
                await getSettings();
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
    }, []);

    const removeFileExtensionFromSettings = useCallback(async (settingsId, fileExtensionId) => {
        setIsLoading(true);
        await apiService
            .removeFileExtensionFromSettings(settingsId, fileExtensionId)
            .then(async () => {
                await getSettings();
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
    }, []);

    const addFileExtensionToSettings = useCallback(async () => {
        var fileExtensionExists = fileExtensionsData.some((x) => x.fileExtension === selectedFileExtension);

        if (selectedFileExtension && fileExtensionExists) {
            var existingFileExtension = fileExtensionsData.filter((x) => x.fileExtension === selectedFileExtension);

            closeModal();
            setIsLoading(true);
            await apiService
                .addFileExtensionToSettings(settingsForModal, existingFileExtension[0].id)
                .then(async () => {
                    await getSettings();
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
            return;
        }

        closeModal();
        setIsLoading(true);
        await apiService
            .addFileExtension(selectedFileExtension)
            .then(async (response) => {
                await apiService.addFileExtensionToSettings(settingsForModal, response.data.id).then(async () => {
                    await getSettings();
                });
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
    }, [selectedFileExtension, settingsForModal, fileExtensionsData]);

    const closeModal = () => {
        setIsOpen(false);
        setSelectedFileExtension({});
        setSettingsForModal(-1);
    };

    const openModal = (settingsId) => {
        setSettingsForModal(settingsId);
        setIsOpen(true);
    };

    const columns = React.useMemo(
        () => [
            {
                Header: 'Id',
                accessor: 'id',
            },
            {
                Header: 'Max file size in bytes',
                accessor: 'maxFileSize',
            },
            {
                Header: 'Max height in Px',
                accessor: 'maxHeight',
            },
            {
                Header: 'Max width in Px',
                accessor: 'maxWidth',
            },
            {
                Header: 'Min height in Px',
                accessor: 'minHeight',
            },
            {
                Header: 'Min width in Px',
                accessor: 'minWidth',
            },
            {
                Header: 'Allowed file extensions',
                accessor: 'allowedFileExtensions',
                Cell: (props) => {
                    const listItems = props.value.map((fileExtension) => (
                        <li style={{ marginBottom: 0 }} key={fileExtension.id}>
                            {fileExtension.fileExtension}
                            <AiFillMinusCircle
                                style={{ verticalAlign: 'middle', marginLeft: 10 }}
                                className="clickable-svg"
                                color={'darkred'}
                                onClick={() => removeFileExtensionFromSettings(props.row.original.id, fileExtension.id)}
                            />
                        </li>
                    ));

                    return <ul style={{ marginBottom: 0 }}>{listItems}</ul>;
                },
            },
            {
                Header: 'Enabled',
                accessor: 'isEnabled',
                Cell: (props) => {
                    return <input type="checkbox" onChange={() => enableSettings(props.row.original.id)} checked={props.value} />;
                },
            },
            {
                Header: 'Remove',
                Cell: (props) => {
                    return (
                        <button className="button button-clear" onClick={() => removeSettings(props.row.original.id)}>
                            Remove
                        </button>
                    );
                },
            },
            {
                Header: 'Add Extension',
                Cell: (props) => {
                    return (
                        <div>
                            <button className="button button-clear" onClick={() => openModal(props.row.original.id)}>
                                Add
                            </button>
                        </div>
                    );
                },
            },
        ],
        [enableSettings, removeSettings, removeFileExtensionFromSettings],
    );

    return (
        <>
            <Navigation />
            <div className="container">
                <p style={{ color: 'darkred' }}>{result}</p>
                <Link to="/createSettings">Add settings</Link>
                {isLoading ? (
                    <div style={{ textAlign: 'center' }}>
                        <Loader type="TailSpin" color="#9b4dca" height={50} width={50} />
                    </div>
                ) : (
                    <>
                        <Table columns={columns} data={settingsData} />
                        <Modal isOpen={modalIsOpen} onRequestClose={() => closeModal()} style={customStyles} contentLabel="Extensions Modal">
                            {isLoading ? (
                                <div style={{ textAlign: 'center' }}>
                                    <Loader type="TailSpin" color="#9b4dca" height={50} width={50} />
                                </div>
                            ) : (
                                <div style={{ textAlign: 'center' }}>
                                    <input placeholder="Select extension or add new one" type="text" list="extensions" onChange={(e) => setSelectedFileExtension(e.target.value)} />
                                    <datalist id="extensions">
                                        {fileExtensionsData.map((x) => {
                                            return <option key={x.id}>{x.fileExtension}</option>;
                                        })}
                                    </datalist>
                                    <button onClick={() => addFileExtensionToSettings()}>Add</button>
                                </div>
                            )}
                        </Modal>
                    </>
                )}
            </div>
        </>
    );
}

export default Settings;
