import React, { useEffect, useState } from 'react';
import logo from './logo.svg';
import './App.css';
import {ChatRoom} from './Components/ChatRoom';
import {Login} from './Components/Login';

function App() {
  const [username, setUsername] = useState("");

  let handleUserSubmission = (u: string) => {
    setUsername(u);
  };

  return (
    <div className="App">
      {username && <ChatRoom username={username}/>}
      {!username && <Login setUsername={handleUserSubmission}/>}
    </div>
  );
}

export default App;
