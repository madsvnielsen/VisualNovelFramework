# Visual Novel Framework for Unity

This repository contains a framework for creating visual novels in Unity. The framework includes classes for managing dialogues, NPCs, choices, backdrops, and sound clips, and it provides a sample scene setup to help you get started quickly.

## Features

- **NPCs**: Represent characters with a name and portrait.
- **Dialogues**: Messages with optional sender and receiver NPCs, choices, backdrops, and sound clips.
- **Choices**: Allow branching dialogue paths and scene transitions.
- **Backdrops**: Changeable background images for different scenes.
- **Sound Clips**: Play audio when dialogues are loaded.

## Getting Started

### Prerequisites

- Unity (version 2020.1 or later recommended)
- Git (for cloning the repository)

### Setup

#### 1. Clone the Repository

1. Open a terminal or command prompt.
2. Navigate to the directory where you want to store the project.
3. Clone the repository.

4. Open Unity Hub.
5. Click on the "Add" button and navigate to the cloned repository folder.
6. Select the folder and click "Open."

#### 2. Open the Project in Unity

1. Once the project is added to Unity Hub, click on the project to open it in Unity.
2. Wait for Unity to load and compile the scripts. This may take a few moments.

### Creating ScriptableObjects

Use the provided ScriptableObject templates to create NPCs, Dialogues, and Choices.

#### Creating NPCs

1. Right-click in the Project window.
2. Navigate to `Create -> Visual Novel -> NPC`.
3. Name the NPC asset and configure the `npcName` and `portrait` in the Inspector.

#### Creating Dialogues

1. Right-click in the Project window.
2. Navigate to `Create -> Visual Novel -> Dialogue`.
3. Name the Dialogue asset and configure the fields in the Inspector:
   - `Sender`: NPC who is sending the message.
   - `Receiver`: NPC who is receiving the message.
   - `Message`: The dialogue message.
   - `Next Dialogue`: The next dialogue to follow.
   - `Choices`: List of choices for branching dialogues.
   - `Backdrop`: Background image for the dialogue.
   - `Sound Clip`: Audio to play when the dialogue is loaded.
   - `Next Scene`: Name of the next scene to load (if applicable).

#### Creating Choices

Choices are created within Dialogue assets.

1. In the Dialogue asset Inspector, expand the `Choices` list.
2. Add elements to the list and configure each choice:
   - `Text`: Text displayed for the choice.
   - `Next Dialogue`: The next dialogue to follow if this choice is selected.
   - `Next Scene`: Name of the next scene to load if this choice is selected.

### Scene Setup

#### Hierarchy

Set up the scene hierarchy as follows:

- **Canvas**
  - **DialoguePanel**
    - **PortraitSender** (Image)
    - **PortraitReceiver** (Image)
    - **MessageText** (Text)
    - **ChoicesPanel** (Vertical Layout Group)
      - **ChoiceButton** (Button) [Instantiate for each choice]
  - **Backdrop** (Image)
- **AudioSource** (For playing sound clips)

#### Components

Attach the following components to the respective GameObjects:

- **Canvas**: `Canvas`, `CanvasScaler`, `GraphicRaycaster`
- **DialoguePanel**: `RectTransform`, `CanvasRenderer`, `Image` (optional for background)
  - **PortraitSender**: `RectTransform`, `CanvasRenderer`, `Image`
  - **PortraitReceiver**: `RectTransform`, `CanvasRenderer`, `Image`
  - **MessageText**: `RectTransform`, `CanvasRenderer`, `Text`
  - **ChoicesPanel**: `RectTransform`, `CanvasRenderer`, `VerticalLayoutGroup`
    - **ChoiceButton**: `RectTransform`, `CanvasRenderer`, `Image`, `Button`, `Text`
- **Backdrop**: `RectTransform`, `CanvasRenderer`, `Image`
- **AudioSource**: `AudioSource`

### DialogueManager Setup

1. Create an empty GameObject and name it `DialogueManager`.
2. Attach the `DialogueManager` script to this GameObject.
3. In the Inspector, assign the respective UI elements (PortraitSender, PortraitReceiver, MessageText, ChoicesPanel, ChoiceButtonPrefab, Backdrop, and AudioSource) to the corresponding fields in the `DialogueManager` script.

### Starting the Dialogue

To start the dialogue, use the `GameController` script:

1. Create an empty GameObject and name it `GameController`.
2. Attach the `GameController` script to this GameObject.
3. In the Inspector, assign the `DialogueManager` and `startingDialogue` (the initial dialogue asset) to the corresponding fields in the `GameController` script.

### Playing the Visual Novel

1. Press the Play button in the Unity Editor.
2. The dialogue system should start, displaying the initial dialogue and allowing you to interact with choices and progress through the story.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
