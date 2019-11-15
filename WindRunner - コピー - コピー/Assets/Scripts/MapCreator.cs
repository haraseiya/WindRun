using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ブロッククラス
public class Block
{
    public enum TYPE // ブロックの状態
    {
        NONE = -1,   // 状態情報なし
        FLOOR = 0,   // 床
        HOLE,        // 穴
        NUM,         // 状態が何種類あるか
    };
};

public class MapCreator : MonoBehaviour
{
    private GameRoot game_root = null;
    public TextAsset level_data_text = null;

    // ブロックの幅
    public static float BLOCK_WIDTH = 1.0f;

    // ブロックの高さ
    public static float BLOCK_HEIGHT = 0.2f;

    // 敵の高さ
    public float ENEMY_HEIGHT = 3.0f;

    // 画面に収まるブロック数
    public static int BLOCK_NUM_IN_SCREEN = 100;

    float time;

    private LevelControl level_control = null;

    private struct FloorBlock
    {
        // ブロックが作成済みか否か
        public bool is_created;

        // ブロックの位置
        public Vector3 position;                    
    }

    // 最後に作成したブロック  
    private FloorBlock last_block;

    // シーン上のPlayerを保管
    private PlayerControl player = null;

    // BlockCreatorを保管
    private BlockCreator block_creator;

    // EnemyCreatorを保管
    private EnemyCreator enemy_creator;

    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        this.last_block.is_created = false;
        this.enemy_creator = this.gameObject.GetComponent<EnemyCreator>();
        this.block_creator = this.gameObject.GetComponent<BlockCreator>();
        this.level_control = new LevelControl();
        this.level_control.initialize();
        this.level_control.loadLevelData(this.level_data_text);
        this.game_root = this.gameObject.GetComponent<GameRoot>();
        this.player.level_control = this.level_control;
    }

    void Update()
    {
        time += Time.deltaTime;
        // プレイヤーのX位置を取得
        float block_generate_x = this.player.transform.position.x;

        // そこから、およそ半画面分、右へ移動
        // この位置が、ブロックを生み出すしきい値になる
        block_generate_x += BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN + 1) / 2.0f;

        // 最後に作ったブロックの位置がしきい値より小さい間ブロックを作る
        while (this.last_block.position.x<block_generate_x)
        {
            this.create_floor_block();

            if (time > 3) 
            {
                this.create_enemy();
                time = 0;
            }
        }
    }

    private void create_enemy()
    {
        // これから作る敵のポジション
        Vector3 enemy_position;

        // 敵の高さをランダムに設定
        ENEMY_HEIGHT = Random.Range(1, 3);

        // last_blockが未作成の場合
        if (!this.last_block.is_created)
        {
            enemy_position = this.player.transform.position;
            enemy_position.x -= BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);
            enemy_position.y = ENEMY_HEIGHT;
        }
        else
        {
            enemy_position = this.last_block.position;
            enemy_position.y = this.last_block.position.y + ENEMY_HEIGHT;
        }

        // 今回作るブロックに関する情報を変換currentに収める
        LevelControl.CreationInfo current = this.level_control.current_block;

        // 今回作るブロックが床なら
        if (current.block_type == Block.TYPE.FLOOR)
        {
            // block_positionの位置に、ブロックを実際に作成
            this.enemy_creator.createEnemy(enemy_position);
        }

        enemy_position.x += BLOCK_WIDTH;
        this.last_block.is_created = true;
    }

    private void create_floor_block()
    {
        // これから作るブロックの位置
        Vector3 block_position;               

        // last_blockが未作成の場合
        if (!this.last_block.is_created)         
        {
            // ブロックの位置を、とりあえずPlayerと同じにする
            block_position = this.player.transform.position;

            // それから、ブロックのX位置を半画面分、左に移動
            block_position.x -= BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);

            block_position.y = 0.0f;
        }
        // last_blockが作成済みの場合
        else
        {
            // 今回作るブロックの位置を、前回作ったブロックと同じ位置に
            block_position = this.last_block.position;
        }

        // ブロックを1ブロック分、右に移動
        block_position.x += BLOCK_WIDTH;

        // this.block_creator.createBlock(block_position);

        //this.level_control.update();
        this.level_control.update(this.game_root.getPlayTime());

        // level_controlに置かれたcurrent_block(今作るブロックの情報)
        // height(高さ)をシーン上の座標に変換
        block_position.y = level_control.current_block.height * BLOCK_HEIGHT;

        // 今回作るブロックに関する情報を変換currentに収める
        LevelControl.CreationInfo current = this.level_control.current_block;

        // 今回作るブロックが床なら
        if(current.block_type==Block.TYPE.FLOOR)
        {
            // block_positionの位置に、ブロックを実際に作成
            this.block_creator.createBlock(block_position);
        }

        this.last_block.position = block_position;

        this.last_block.is_created = true;
    }

    // ブロックを消すかどうか
    public bool isDelete_enemy(GameObject enemy_object)
    {
        // 戻り値
        bool ret = false;

        // Playerから、半画面分、左の位置
        // これが、消えるべきか否かを決めるしきい値となる
        float left_limit = this.player.transform.position.x - BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);

        // ブロックの位置がしきい値より小さい（左）なら
        if (enemy_object.transform.position.x < left_limit)
        {
            // 戻り値をtrue（消えてよし）に
            ret = true;
        }

        // 判定結果を戻す
        return (ret);
    }

    // ブロックを消すかどうか
    public bool isDelete_block(GameObject block_object)
    {
        // 戻り値
        bool ret = false;

        // Playerから、半画面分、左の位置
        // これが、消えるべきか否かを決めるしきい値となる
        float left_limit = this.player.transform.position.x - BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);

        // ブロックの位置がしきい値より小さい（左）なら
        if(block_object.transform.position.x<left_limit)
        {
            // 戻り値をtrue（消えてよし）に
            ret = true;
        }

        // 判定結果を戻す
        return (ret);
    }
}

