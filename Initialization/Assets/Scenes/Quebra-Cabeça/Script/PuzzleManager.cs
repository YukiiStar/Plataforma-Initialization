using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


// Este script deve ser colocado em um GameObject vazio chamado "GameManager" ou similar
public class PuzzleManager : MonoBehaviour
{
   public Transform puzzleGrid; // Painel que contém os botões (peças do puzzle)
   public Button undoButton;    // Botão de desfazer jogada
   public Button replayButton;  // Botão de assistir replay
   public Button skipButton;    // Botão de pular o replay


   private List<ICommand2> commandHistory = new List<ICommand2>(); // Armazena todas as jogadas feitas (para replay)
   private Stack<ICommand2> undoStack = new Stack<ICommand2>();    // Armazena comandos que podem ser desfeitos


   private List<Piece> pieces = new List<Piece>(); // Lista de todas as peças no tabuleiro
   private Piece firstSelected = null; // Guarda a primeira peça clicada


   private bool isReplaying = false;   // Flag para saber se o replay está em execução


   void Start()
   {
       replayButton.gameObject.SetActive(false);
       skipButton.gameObject.SetActive(false);


       SetupPieces();      // Configura as peças e seus índices
       ShufflePieces();    // Embaralha a ordem inicial das peças
   }


   // Associa cada peça ao seu índice correto
   void SetupPieces()
   {
       pieces.Clear();


       for (int i = 0; i < puzzleGrid.childCount; i++)
       {
           Transform t = puzzleGrid.GetChild(i);
           Piece p = t.GetComponent<Piece>();
           p.manager = this;
           p.SetIndex(i); // Define o índice correto dessa peça
           pieces.Add(p);
       }
   }


   // Embaralha a ordem das peças no início
   void ShufflePieces()
   {
       for (int i = 0; i < pieces.Count; i++)
       {
           int randomIndex = Random.Range(0, pieces.Count);
           Transform a = pieces[i].transform;
           Transform b = pieces[randomIndex].transform;


           // Troca a posição visual no grid (Canvas)
           int indexA = a.GetSiblingIndex();
           int indexB = b.GetSiblingIndex();
           a.SetSiblingIndex(indexB);
           b.SetSiblingIndex(indexA);
       }
/*
       // 🛠 Atualiza a lista pieces com a nova ordem visual dos botões
       pieces.Clear();
       for (int i = 0; i < puzzleGrid.childCount; i++)
       {
           pieces.Add(puzzleGrid.GetChild(i).GetComponent<Piece>());
       }*/
   }


   // Chamado quando uma peça é clicada
   public void OnPieceClicked(Piece clicked)
   {
       if (isReplaying) return; // Ignora cliques durante o replay


       if (firstSelected == null)
       {
           firstSelected = clicked; // Seleciona a primeira peça
       }
       else
       {
           // Segunda peça clicada → troca as peças
           Transform a = firstSelected.transform;
           Transform b = clicked.transform;


           int indexA = a.GetSiblingIndex();
           int indexB = b.GetSiblingIndex();


           // Cria o comando e executa
           SwapCommand cmd = new SwapCommand(a, b, indexA, indexB);
           cmd.Execute();
           
           commandHistory.Add(cmd);
           undoStack.Push(cmd);

           firstSelected = null;
           CheckWin();
       }
   }
   
   // Verifica se todas as peças estão no lugar correto
   void CheckWin()
   {
       Debug.Log(puzzleGrid.childCount);
       for (int i = 0; i < pieces.Count; i++)
       {
           Piece[] pieces = puzzleGrid.GetComponentsInChildren<Piece>();
           if (pieces[i].correctIndex != i)
               return; // Se uma peça está fora do lugar, ainda não ganhou
       }


       // Se passou por todas, o puzzle foi resolvido!
       Debug.Log("🎉 Quebra-cabeça completo!");


       // Ativa opções de replay
       replayButton.gameObject.SetActive(true);
       skipButton.gameObject.SetActive(true);
   }


   // Chamado pelo botão de desfazer
   public void Undo()
   {
       if (isReplaying || undoStack.Count == 0 || firstSelected != null) return;


       ICommand2 lastCommand = undoStack.Pop(); // Remove o último comando
       lastCommand.Undo();                     // Desfaz a troca
   }


   // Chamado ao clicar em "Ver Replay"
   public void StartReplay()
   {
       if (isReplaying) return;


       StartCoroutine(ReplaySequence()); // Inicia a rotina do replay
   }


   // Coroutine que executa o replay, com delay de 1 segundo entre as jogadas
   IEnumerator ReplaySequence()
   {
       isReplaying = true;
       skipButton.gameObject.SetActive(true);


       // Reinicia o puzzle embaralhado
       ShufflePieces();


       // Espera um pouco antes de começar o replay
       yield return new WaitForSeconds(1f);


       foreach (ICommand2 cmd in commandHistory)
       {
           cmd.Execute();
           yield return new WaitForSeconds(1f); // Espera 1 segundo entre cada jogada
       }


       isReplaying = false;
       skipButton.gameObject.SetActive(false);


       Debug.Log("✅ Replay finalizado!");
   }


   // Chamado ao clicar em "Pular"
   public void SkipReplay()
   {
       StopAllCoroutines(); // Interrompe a animação do replay
       isReplaying = true;


       // Executa todas as jogadas instantaneamente
       foreach (ICommand2 cmd in commandHistory)
       {
           cmd.Execute();
       }


       isReplaying = false;
       skipButton.gameObject.SetActive(false);
       Debug.Log("⏩ Replay pulado!");
   }
   
   public void Undo()
   {
       // Impede desfazer se estiver em replay ou se houver uma peça selecionada
       if (isReplaying || undoStack.Count == 0 || firstSelected != null)
           return;

       ICommand2 cmd = undoStack.Pop();
       cmd.Undo();
   }

}     
  