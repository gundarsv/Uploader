import React from 'react';
import Navigation from '../components/navigation';
import Loader from 'react-loader-spinner';
import * as apiService from '../services/apiService';

export default function CreateSettings() {
    const [result, setResult] = React.useState('');
    const [isLoading, setIsLoading] = React.useState(false);
    const [maxFileSize, setMaxFileSize] = React.useState();
    const [maxHeight, setMaxHeight] = React.useState('');
    const [maxWidth, setMaxWidth] = React.useState('');
    const [minHeight, setMinHeight] = React.useState('');
    const [minWidth, setMinWidth] = React.useState('');

    async function addSettings(maxFileSize, maxHeight, maxWidth, minHeight, minWidth) {
        setIsLoading(true);
        await apiService
            .addSettings(maxFileSize, maxHeight, maxWidth, minHeight, minWidth)
            .then((r) => {
                setResult('Settings were added');
            })
            .catch((e) => {
                setResult(`${e}`);
            });

        setIsLoading(false);
    }

    return (
        <>
            <Navigation />
            <div className="container">
                {isLoading ? (
                    <div style={{ textAlign: 'center' }}>
                        <Loader type="TailSpin" color="#9b4dca" height={50} width={50} />
                    </div>
                ) : (
                    <div>
                        <p style={{ color: 'darkred' }}>{result}</p>
                        <fieldset>
                            <label htmlFor="maxFileSizeField">Max file size in bytes</label>
                            <input type="number" value={maxFileSize} id="maxFileSizeField" onChange={(e) => setMaxFileSize(e.target.value)} />
                            <label htmlFor="maxHeightField">Max height in Px</label>
                            <input type="number" value={maxHeight} id="maxHeightField" onChange={(e) => setMaxHeight(e.target.value)} />
                            <label htmlFor="maxWidthField">Max width in Px</label>
                            <input type="number" value={maxWidth} id="maxWidthField" onChange={(e) => setMaxWidth(e.target.value)} />
                            <label htmlFor="minHeightField">Min height in Px</label>
                            <input type="number" value={minHeight} id="minHeightField" onChange={(e) => setMinHeight(e.target.value)} />
                            <label htmlFor="minWidthField">Min width in Px</label>
                            <input type="number" value={minWidth} id="minWidthField" onChange={(e) => setMinWidth(e.target.value)} />
                            <input className="button-primary float-right" type="submit" value="Add Settings" onClick={() => addSettings(maxFileSize, maxHeight, maxWidth, minHeight, minWidth)} />
                        </fieldset>
                    </div>
                )}
            </div>
        </>
    );
}
