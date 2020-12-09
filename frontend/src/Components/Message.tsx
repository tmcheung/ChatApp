import React from 'react';

interface SentMessage{
    messageContent: string,
    username: string
}

export function Message(message: SentMessage) {
    return (
      <div>
          {message.messageContent} <i>({message.username})</i>
      </div>
    );
}
