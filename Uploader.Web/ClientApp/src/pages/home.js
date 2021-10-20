import React, { useState } from 'react';
import Navigation from '../components/navigation';
import * as apiService from '../services/apiService';
import InfiniteScroll from 'react-infinite-scroll-component';
import Loader from 'react-loader-spinner';
import fileDownload from 'js-file-download';
import { Card } from 'react-bootstrap';
import { Link } from 'react-router-dom';

function Home() {
    const [hasMore, setHasMore] = useState(true);
    const [page, setPage] = useState(0);
    const [fileData, setFileData] = useState([]);
    const [result, setResult] = React.useState('');

    React.useEffect(() => {
        getFiles(1);
    }, []);

    async function getFiles(pageId) {
        await apiService
            .getFiles(pageId, 10)
            .then((response) => {
                setHasMore(response.data.currentPage < response.data.totalPages);
                setPage(response.data.currentPage);
                setFileData((currentFiles) => [...currentFiles, ...response.data.items]);
            })
            .catch((e) => {
                setResult(`${e}`);
            });
    }

    async function getFile(id, fileName) {
        await apiService
            .getFile(id)
            .then((response) => {
                fileDownload(response.data, fileName);
            })
            .catch((e) => {
                setResult(`${e}`);
            });
    }

    async function removeFile(id) {
        await apiService
            .removeFile(id)
            .then((response) => {
                if (response.status === 200) {
                    setFileData(
                        fileData.filter(function (file) {
                            return file.id !== id;
                        }),
                    );
                }
            })
            .catch((e) => {
                setResult(`${e}`);
            });
    }

    return (
        <>
            <Navigation />
            <div className="container">
                <p style={{ color: 'darkred' }}>{result}</p>
                <Link to="/upload">Upload files</Link>
                <InfiniteScroll
                    dataLength={fileData.length}
                    next={() => getFiles(page + 1)}
                    hasMore={hasMore}
                    loader={
                        <div style={{ textAlign: 'center' }}>
                            <Loader type="TailSpin" color="#9b4dca" height={50} width={50} />
                        </div>
                    }>
                    <div>
                        {fileData.map((file, index) => (
                            <Card className="text-center" key={index}>
                                <Card.Header>File Id: {`${file.id}`}</Card.Header>
                                <Card.Body>
                                    <Card.Title>{`${file.fileName}.${file.extension}`}</Card.Title>
                                    <Card.Text>{file.comments}</Card.Text>
                                    <a href="/#" onClick={() => getFile(file.id, `${file.fileName}.${file.extension}`)}>
                                        Dowload
                                    </a>
                                    <a href="/#" style={{ marginLeft: 10 }} onClick={() => removeFile(file.id)}>
                                        Remove
                                    </a>
                                </Card.Body>
                            </Card>
                        ))}
                    </div>
                </InfiniteScroll>
            </div>
        </>
    );
}

export default Home;
