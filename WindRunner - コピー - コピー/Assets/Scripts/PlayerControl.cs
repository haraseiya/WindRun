using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static float ACCELERATION = 10.0f;               // 加速度
    public static float SPEED_MIN = 4.0f;                   // 速度の最小値
    public static float SPEED_MAX = 8.0f;                   // 速度の最大値
    public static float JUMP_HEIGHT_MAX = 3.0f;             // ジャンプの高さ
    public static float JUMP_KEY_RELEASE_REDUCE = 0.5f;     // ジャンプからの減速値
    public static float NARAKU_HEIGHT = -2.0f;              // しきい値
    public float current_speed = 0.0f;                      // 現在のスピード
    public float lapseTime = 0.0f;                          // クールタイム
    private float CLICK_GRACE_TIME = 0.5f;                  // 「ジャンプしたい意志」を受け付ける時間
    public bool isAttackable = true;                        // 攻撃できるかどうか
    float time = 0.0f;                                      // 経過時間

    public GameObject[] bulletPrefab;                       // 弾のプレハブを格納

    public LevelControl level_control = null;               // LevelControlを保持
    public Result result = null;                            // Rsultを保持
    private Animator anim = null;                           // アニメーターを格納

    public enum STEP    // Playerの各種状態を表すデータ型
    {
        NONE=-1,        // 状態情報なし
        RUN=0,          // 走る
        JUMP,           // ジャンプ
        DOUBLEJUMP,     // 2段ジャンプ
        SHOT1,          // ショット1
        SHOT2,          // ショット2
        MISS,           // ミス
        NUM,            // 状態が何種類あるかを示す（=3）
    };

    public STEP step = STEP.NONE;           // Playerの現在の状態
    public STEP next_step = STEP.NONE;      // Playerの次の状態

    [SerializeField]
    bool is_landed = false;                 // 着地しているかどうか

    public float step_timer = 0.0f;         // 経過時間
    private bool is_collided = false;       // 何かとぶつかっているかどうか
    private bool is_key_released = false;   // ボタンが離されているかどうか

    void Start()
    {
        this.next_step = STEP.RUN;          // RUN状態から始まる
        anim = GetComponent<Animator>();    // アニメーターを取得
        result = GameObject.Find("GameRoot").GetComponent<Result>();
    }

    void Update()
    {
        // 速度を設定
        Vector3 velocity = this.GetComponent<Rigidbody>().velocity; 

        // 現在のスピードをレベルに合わせる
        this.current_speed = this.level_control.getPlayerSpeed();

        // 着地状態かどうかをチェック
        this.check_landed();

        // ショット間のクールタイム
        this.cool_time();

        // 時間
        time += Time.deltaTime;

        // 弾のポジション
        Vector3 shotPos;
        shotPos = this.transform.position;
        shotPos.x = this.transform.position.x + 1.0f;
        shotPos.y = this.transform.position.y + 1.0f;

        switch (this.step)
        {
            // 走る状態
            case STEP.RUN:

            // 火の玉攻撃状態
            case STEP.SHOT1:

            // 氷の玉攻撃状態
            case STEP.SHOT2:

            // ジャンプ状態
            case STEP.JUMP:

            // ２段ジャンプ状態
            case STEP.DOUBLEJUMP:

                // 落ちたら
                if (this.transform.position.y < NARAKU_HEIGHT)
                {
                    // 「ミス」状態にする
                    this.next_step = STEP.MISS;
                }

                break;
        }

        // 「次の状態」が決まっていなければ、状態の変化を調べる
        if(this.next_step==STEP.NONE)
        {
            // Playerの現在の状態で分岐
            switch (this.step)                               
            {
                // 走行中の場合
                case STEP.RUN:

                    // ジャンプボタンが押されたら
                    if (Input.GetButtonDown("Jump"))
                    {
                        // 次の状態をジャンプに変更
                        this.next_step = STEP.JUMP;
                    }

                    // Shot1ボタンが押されたら
                    if (isAttackable == true && Input.GetButtonDown("Shot1")) 
                    {
                        // 次の状態をショット1に変更
                        this.next_step = STEP.SHOT1;
                    }

                    // Shot2ボタンが押されたら
                    if(isAttackable == true && Input.GetButtonDown("Shot2"))
                    {
                        // 次の状態をショット2に変更
                        this.next_step = STEP.SHOT2;
                    }

                    break;

                // ジャンプ中の場合
                case STEP.JUMP:

                    // 着地したら
                    if (this.is_landed)
                    {
                        // 次の状態を走行中に変更
                        this.next_step = STEP.RUN;
                    }

                    // ジャンプボタンが押されたら
                    if (Input.GetButtonUp("Jump"))
                    {
                        // 次の状態を２段ジャンプ中に変更
                        this.next_step = STEP.DOUBLEJUMP;
                    }

                    // Shot1ボタンが押されたら
                    if (Input.GetButtonDown("Shot1"))
                    {
                        // 次の状態をショット1に変更
                        this.next_step = STEP.SHOT1;
                    }

                    // Shot2ボタンが押されたら
                    if (Input.GetButtonDown("Shot2"))
                    {
                        // 次の状態をショット2に変更
                        this.next_step = STEP.SHOT2;
                    }

                    break;

                // ２段ジャンプ中
                case STEP.DOUBLEJUMP:

                    // 着地したら
                    if(this.is_landed)
                    {
                        // 次の状態を走行中に変更
                        this.next_step = STEP.RUN;
                    }

                    // Shot1ボタンが押されたら
                    if (Input.GetButtonDown("Shot1"))
                    {
                        // 次の状態をショット1に変更
                        this.next_step = STEP.SHOT1;
                    }

                    // Shot2ボタンが押されたら
                    if (Input.GetButtonDown("Shot2"))
                    {
                        // 次の状態をショット2に変更
                        this.next_step = STEP.SHOT2;
                    }

                    break;

                // ショット1状態の場合
                case STEP.SHOT1:

                    // 着地していたら
                    if (this.is_landed)
                    {
                        // 次の状態を走るに変更
                        this.next_step = STEP.RUN;
                    }

                    // ジャンプ
                    if (Input.GetButtonDown("Jump"))
                    {
                        // 次の状態をジャンプに変更
                        this.next_step = STEP.JUMP;
                    }

                    break;

                // ショット2状態の場合
                case STEP.SHOT2:

                    // 着地していたら
                    if (this.is_landed)
                    {
                        // 次の状態を走るに変更
                        this.next_step = STEP.RUN;
                    }

                    // ジャンプ
                    if (Input.GetButtonDown("Jump"))
                    {
                        // 次の状態をジャンプに変更
                        this.next_step = STEP.JUMP;
                    }

                    break;
            }
        }

        // 「次の状態」が「状態情報なし」以外の間
        while(this.next_step!=STEP.NONE)
        {
            // 「現在の状態」を「次の状態」に更新
            this.step = this.next_step;

            // 「次の状態」を「状態なし」に変更
            this.next_step = STEP.NONE;

            // 更新された「現在の状態」が
            switch (this.step)               
            {
                // 走行中
                case STEP.RUN:

                    anim.SetBool("Jump", false);
                    anim.SetBool("DoubleJump", false);

                    break;

                // ジャンプ中
                case STEP.JUMP:

                    // ジャンプ初動
                    velocity.y = Mathf.Sqrt(2.0f * 9.8f * PlayerControl.JUMP_HEIGHT_MAX);

                    // ジャンプアニメーションフラグON
                    anim.SetBool("Jump", true);

                    // 「ボタンが離されたフラグ」をクリアする
                    this.is_key_released = false;

                    break;

                // 二段ジャンプ中
                case STEP.DOUBLEJUMP:

                    // ジャンプ初動
                    velocity.y = Mathf.Sqrt(2.0f * 9.8f * PlayerControl.JUMP_HEIGHT_MAX);

                    // ２段ジャンプアニメーションフラグON
                    anim.SetBool("DoubleJump", true);

                    // 「ボタンが離されたフラグ」をクリアする
                    this.is_key_released = false;

                    break;

                case STEP.SHOT1:

                    anim.SetBool("Attack", true);
                    GameObject go = GameObject.Instantiate(bulletPrefab[0]) as GameObject;
                    go.transform.position = shotPos;
                    isAttackable = false;

                    break;

                case STEP.SHOT2:

                    anim.SetBool("Attack", true);
                    go = GameObject.Instantiate(bulletPrefab[1]) as GameObject;
                    go.transform.position = shotPos;
                    isAttackable = false;

                    break;

            }  
        }

        // 状態ごとの、毎フレームの更新処理
        switch(this.step)
        {
            // 走行中の場合
            case STEP.RUN:

                // プレイヤースピード
                velocity.x = current_speed;
                break;

            // ジャンプ中の場合
            case STEP.JUMP:     

                do
                {
                    // 「ボタンが離された瞬間」じゃなかったら
                    if (!Input.GetButtonUp("Jump"))
                    {
                        // 何もせずにループを抜ける
                        break;  
                    }

                    // 減速済みなら（2回以上減速しないように）
                    if (this.is_key_released)
                    {
                        break;  
                    }

                    // 上下方向の速度が0以下なら（下降中なら）
                    if (velocity.y <= 0.0f)
                    {
                        break;  
                    }

                    // ボタンが離されていて、上昇中なら、減速開始
                    // ジャンプの上昇はここでおしまい
                    velocity.y *= JUMP_KEY_RELEASE_REDUCE;

                    this.is_key_released = true;
                } while (false);
                break;

            // 2段ジャンプ中の場合
            case STEP.DOUBLEJUMP:

                do
                {
                    // 「ボタンが離された瞬間」じゃなかったら
                    if (!Input.GetButtonUp("Jump"))
                    {
                        // 何もせずにループを抜ける
                        break;
                    }

                    // 減速済みなら（2回以上減速しないように）
                    if (this.is_key_released)
                    {
                        break;
                    }

                    // 上下方向の速度が0以下なら（下降中なら）
                    if (velocity.y <= 0.0f)
                    {
                        break;
                    }

                    // ボタンが離されていて、上昇中なら、減速開始
                    // ジャンプの上昇はここでおしまい
                    velocity.y *= JUMP_KEY_RELEASE_REDUCE;

                    this.is_key_released = true;
                } while (false);
                break;

            // ミスした場合
            case STEP.MISS:

                // 加速値(ACCELERATION)を引き算して、Playerの速度を遅くしていく
                velocity.x -= ACCELERATION * Time.deltaTime;

                // Playerの速度が負の数なら
                if (velocity.x<0.0f)     
                {
                    // ゼロにする
                    velocity.x = 0.0f;  
                }
                break;
        }

        // Rigidbodyの速度を、上記で求めた速度で更新
        // （この行は、状態にかかわらず毎回実行される）
        this.GetComponent<Rigidbody>().velocity = velocity;
    }

    // 接地しているか確認
    private void check_landed()
    {
        this.is_landed = false;     

        do
        {
            // Playerの現在の位置
            Vector3 player_position = this.transform.position;

            // プレイヤーからプレイヤー下に1.0f移動した位置
            Vector3 player_position_down = player_position + Vector3.down * 0.2f;    

            RaycastHit hit;

            // プレイヤー位置からプレイヤー下0.1の間に何もない場合
            if (!Physics.Linecast(player_position, player_position_down, out hit))
            {
                // 何もせずdo∼whileループを抜ける
                break;  
            }

            //  sからeの間に何かがあり、JUMP直後でない場合のみ、以下が実行される
            this.is_landed = true;
        } while (false);
    }

    private void cool_time()
    {
        // 直前のフレームからの経過時間を足す
        if (isAttackable == false)
        {
            lapseTime += Time.deltaTime;

            if (lapseTime >= 0.5f)
            {
                isAttackable = true;
                lapseTime = 0.0f;
                anim.SetBool("Attack", false);
            }
        }
    }

    // プレイヤーと敵がぶつかったとき
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Enemy")
        {
            this.next_step = STEP.MISS;
        }
    }

    // ゲームが終わったかどうかを判定する
    public bool isPlayEnd()     
    {
        bool ret = false;

        switch(this.step)
        {
            // ミス状態なら
            case STEP.MISS:

                // 「死んだよ」とtrueを返す
                ret = true;     
                break;
        }
        return (ret);
    }
}
