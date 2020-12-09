import React, { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { Message } from './Message';
import * as signalR from "@microsoft/signalr";

const apiUrl = "http://localhost:60972";

export function ChatRoom({username}) {
    const [inp, setInput] = useState("");
    
    const [chat, setChat] = useState([]);
    const chatRef = useRef([]);
    chatRef.current = chat;

    const [members, setMembers] = useState([]);

    const connection = useMemo(() =>
        new signalR.HubConnectionBuilder()
        .withUrl(`${apiUrl}/hubs/chat`)
        .withAutomaticReconnect()
        .build()
    , []);

    useEffect(() => {
        connection.start()
            .then(result => {
                connection.on('ReceiveMembershipChange', message => {
                    handleMembership(message);
                });

                connection.on('ReceiveMessage', message => {
                    handleMessage(message);
                });

                if (connection.connectionId) {
                    connection.invoke("JoinChat", username).then(r => {
                        console.log('joined chat ', r);
                    }).catch(er => console.log('failed to join chat ', er));
                }  
            })
            .catch(e => console.log('Connection failed: ', e));
    }, []);

    let handleMessage = (message) => {
        console.log('ReceiveMessage: ', message);
        const updatedChat = [...chatRef.current];
        updatedChat.push(message);
        console.log(updatedChat)
        setChat(updatedChat);
    }

    let handleMembership = (message) => {
        console.log('ReceiveMembershipChange: ',message);
        setMembers(message);
    }

    let handlePost = (text: string) => {
        if (connection != null && connection.connectionId) {
            connection.invoke("SendMessage", text).then(r => {
                setInput("");
            }).catch(er => console.log('failed to send message ', er));
        }else{
            console.log("not connected")
        }
    }

    return (
        <React.Fragment>
            Users online: {members.toString()}
            <hr/>
            <input value={inp} onChange={(e) => setInput(e.target.value)}/>
            <button onClick={() => handlePost(inp)}>Send</button>
            <hr/>
            {chat.map(m =>
                <Message messageContent={m.messageContent} username={m.username} />
            )}
        </React.Fragment>
    );
}

