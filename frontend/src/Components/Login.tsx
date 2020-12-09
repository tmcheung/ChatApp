import React, { useState } from 'react';

export function Login({setUsername}) {
    const [inp, setInput] = useState("");

    return (
        <div>
            <input type="text" value={inp} onChange={(e) => setInput(e.target.value)}/>
            <button onClick={() => setUsername(inp)}>Start chatting</button>
        </div>
    );
}