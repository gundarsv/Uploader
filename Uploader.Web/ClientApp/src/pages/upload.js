import React from 'react';
import FileUploader from '../components/fileUploader';
import Navigation from '../components/navigation';

export default function Upload() {
    return (
        <>
            <Navigation />
            <div className="container">
                <FileUploader />
            </div>
        </>
    );
}
